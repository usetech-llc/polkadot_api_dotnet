using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using OneOf;
using Polkadot.BinarySerializer.Extensions;

namespace Polkadot.BinarySerializer
{
    public class BinarySerializer : IBinarySerializer
    {
        private readonly Dictionary<Type, IBinaryConverter> _convertersCache = new Dictionary<Type, IBinaryConverter>();
        private readonly Dictionary<Type, ICollection<OneOf<FieldInfo, PropertyInfo>>> _serializableMembersCache = new Dictionary<Type, ICollection<OneOf<FieldInfo, PropertyInfo>>>();
        private readonly Dictionary<Type, ICollection<OneOf<FieldInfo, PropertyInfo>>> _deserializableMembersCache = new Dictionary<Type, ICollection<OneOf<FieldInfo, PropertyInfo>>>();

        private readonly Dictionary<(byte module, byte method), Type> _callTypeCache = new Dictionary<(byte module, byte method), Type>();
        private readonly Dictionary<Type, (byte module, byte method)> _callIndexCache = new Dictionary<Type, (byte module, byte method)>();
        private readonly Dictionary<(byte module, byte @event), Type> _eventTypeCache = new Dictionary<(byte module, byte @event), Type>();
        private readonly Dictionary<Type, (byte module, byte @event)> _eventIndexCache = new Dictionary<Type, (byte module, byte @event)>();
        private readonly Dictionary<Type, (byte[] DestPublicKey, byte[] Selector)> _contractMetaCache;
        private readonly Dictionary<(byte[] DestPublicKey, byte[] Selector), Type> _contractTypeCache;

        public BinarySerializer()
        {
        }
        
        public BinarySerializer(IndexResolver resolver, SerializerSettings settings)
        {
            foreach (var (module, method, type) in settings.KnownCalls)
            {
                var callIndex = resolver.CallIndex((module, method));
                if (callIndex.HasValue)
                {
                    _callIndexCache[type] = callIndex.Value;
                    _callTypeCache[callIndex.Value] = type;
                }
            }

            foreach (var (module, @event, type) in settings.KnownEvents)
            {
                var eventIndex = resolver.EventIndex((module, @event));
                if (eventIndex.HasValue)
                {
                    _eventIndexCache[type] = eventIndex.Value;
                    _eventTypeCache[eventIndex.Value] = type;
                }
            }

            _contractMetaCache = settings.KnownContractCalls
                .ToDictionary(c => c.type, c => (c.DestPublicKey, c.Selector));

            _contractTypeCache = settings.KnownContractCalls
                .ToDictionary(c => (c.DestPublicKey, c.Selector), c => c.type, new ContractSelectorComparer());
        }

        public byte[] Serialize<T>(T value)
        {
            using var ms = new MemoryStream();
            Serialize(value, ms);
            return ms.ToArray();
        }

        public void Serialize<T>(T value, Stream stream)
        {
            SerializeInternal(value, stream);
        }

        public T Deserialize<T>(Stream stream)
        {
            return (T) DeserializeInternal(typeof(T), stream);
        }

        public object Deserialize(Type type, Stream stream)
        {
            return DeserializeInternal(type, stream);
        }

        private object DeserializeInternal(Type type, Stream stream)
        {
            if (typeof(IBinaryDeserializable).IsAssignableFrom(type))
            {
                var deserializable = (IBinaryDeserializable)CreateObject(type);
                return deserializable.Deserialize(stream, this);
            }
            
            return type switch
            {
                {} when type == typeof(byte) => stream.ReadByteThrowIfStreamEnd(),
                {} when type == typeof(sbyte) => (sbyte)stream.ReadByteThrowIfStreamEnd(),
                {} when type == typeof(short) => ReadShort(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()),
                {} when type == typeof(ushort) => (ushort)ReadShort(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()),
                {} when type == typeof(int) => ReadInt(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()),
                {} when type == typeof(uint) => (uint)ReadInt(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()),
                {} when type == typeof(long) => ReadLong(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()),
                {} when type == typeof(ulong) => (ulong)ReadLong(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()),
                {} when type == typeof(bool) => ReadBool(stream.ReadByteThrowIfStreamEnd()),
                {} when typeof(Enum).IsAssignableFrom(type) => ReadEnum(type, stream), 
                
                _ => ReadObject(type, stream),
            };

        }

        private bool ReadBool(byte b)
        {
            if (b == 0)
            {
                return false;
            }

            if (b == 1)
            {
                return true;
            }
            
            throw new ArgumentException($"Invalid encoded boolean value: {b}");
        }

        private object ReadEnum(Type type, Stream stream)
        {
            var enumType = Enum.GetUnderlyingType(type);
            return DeserializeInternal(enumType, stream);
        }

        private object ReadObject(Type type, Stream stream)
        {
            var deserialized = CreateObject(type);

            var members = GetDeserializableMembers(type);
            foreach (var member in members)
            {
                try
                {
                    object value = null;
                    var (converter, param) = GetConverter(member);
                    var memberType = member.Match(f => f.FieldType, p => p.PropertyType);
                    if (converter != null)
                    {
                        value = converter.Deserialize(memberType, stream, this, param);
                    }
                    else
                    {
                        value = DeserializeInternal(memberType, stream);
                    }

                    member.Switch(f => f.SetValue(deserialized, value), p => p.SetValue(deserialized, value));
                }
                catch (Exception ex)
                {
                    var memberName = member.Match(f => f.Name, p => p.Name);
                    throw new DeserializationException($"Failed to deserialize {memberName} of {type.FullName}", ex);
                }
            }

            return deserialized;
        }

        private ICollection<OneOf<FieldInfo, PropertyInfo>> GetDeserializableMembers(Type type)
        {
            if (_deserializableMembersCache.TryGetValue(type, out var cachedMembers))
            {
                return cachedMembers;
            }

            var serializableMembers = GetAttributedMembers<SerializeAttribute>(type);
            var members = serializableMembers
                .OrderBy(m => m.attribute.DeserializeOrder ?? m.attribute.Order)
                .Select(m => m.memberInfo)
                .ToList();
            _deserializableMembersCache[type] = members;
            return members;
        }

        public object CreateObject(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            
            var constructorInfo = type.GetConstructor(Array.Empty<Type>());
            if (constructorInfo == null)
            {
                throw new NotSupportedException(
                    $"Type {type.FullName} doesn't have parameterless constructor, cannot deserialize.");
            }

            return constructorInfo.Invoke(Array.Empty<object>());
        }

        public Type GetCallType(byte moduleIndex, byte methodIndex)
        {
            return _callTypeCache[(moduleIndex, methodIndex)];
        }

        public (byte moduleIndex, byte methodIndex) GetCallIndex(Type typeOfCall)
        {
            return _callIndexCache[typeOfCall];
        }

        public Type GetEventType(byte moduleIndex, byte eventIndex)
        {
            return _eventTypeCache[(moduleIndex, eventIndex)];
        }

        public (byte moduleIndex, byte eventIndex) GetEventIndex(Type typeOfEvent)
        {
            return _eventIndexCache[typeOfEvent];
        }

        public Type GetContractParameterType(byte[] destPublicKey, byte[] data)
        {
            return _contractTypeCache[(destPublicKey, data)];
        }

        public (byte[] destPublicKey, byte[] selector) GetContractMeta(Type typeOfParameter)
        {
            return _contractMetaCache[typeOfParameter];
        }

        private long ReadLong(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
        {
            var ul0 = (ulong) b0;
            var ul1 = ((ulong) b1) << 8;
            var ul2 = ((ulong) b2) << 16;
            var ul3 = ((ulong) b3) << 24;
            var ul4 = ((ulong) b4) << 32;
            var ul5 = ((ulong) b5) << 40;
            var ul6 = ((ulong) b6) << 48;
            var ul7 = ((ulong) b7) << 56;

            return (long) (ul0 | ul1 | ul2 | ul3 | ul4 | ul5 | ul6 | ul7);
        }

        private int ReadInt(byte b0, byte b1, byte b2, byte b3)
        {
            var ul0 = (ulong) b0;
            var ul1 = ((ulong) b1) << 8;
            var ul2 = ((ulong) b2) << 16;
            var ul3 = ((ulong) b3) << 24;

            return (int) (ul0 | ul1 | ul2 | ul3);
        }

        private short ReadShort(byte b0, byte b1)
        {
            var ul0 = (ulong) b0;
            var ul1 = ((ulong) b1) << 8;
            return (short)(ul0 | ul1);
        }

        public T Deserialize<T>(byte[] data)
        {
            using var stream = new MemoryStream(data);
            return Deserialize<T>(stream);
        }

        private void SerializeInternal(object value, Stream ms)
        {
            if (value == null)
            {
                return;
            }

            var type = value.GetType();
            
            if (typeof(IBinarySerializable).IsAssignableFrom(type))
            {
                ((IBinarySerializable)value).Serialize(ms, this);
                return;
            }

            if (value is string s)
            {
                WriteString(ms, s);
                return;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                WriteEnumerable((IEnumerable)value, ms);
                return;
            }
            
            switch (value)
            {
                case byte b: 
                    ms.WriteByte(b);
                    break;
                case sbyte b: 
                    ms.WriteByte((byte)b);
                    break;
                case short b:
                    WriteShort(ms, b);
                    break;
                case ushort b:
                    WriteShort(ms, (short)b);
                    break;
                case int b:
                    WriteInt(ms, b);
                    break;
                case uint b:
                    WriteInt(ms, (int)b);
                    break;
                case long b:
                    WriteLong(ms, b);
                    break;
                case ulong b:
                    WriteLong(ms, (long)b);
                    break;
                case Enum e:
                    WriteEnum(ms, e);
                    break;
                case bool b:
                    WriteBool(ms, b);
                    break;
                
                default:
                    WriteObject(ms, type, value);
                    break;
            }
        }

        private void WriteString(Stream ms, string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            ms.Write(bytes, 0, bytes.Length);
        }

        private void WriteBool(Stream ms, bool b)
        {
            if (b)
            {
                ms.WriteByte(1);
            }
            else
            {
                ms.WriteByte(0);
            }
        }

        private void WriteEnumerable(IEnumerable enumerable, Stream ms)
        {
            if (enumerable is byte[] byteArray)
            {
                ms.Write(byteArray, 0, byteArray.Length);
                return;
            }
            
            foreach (var item in enumerable)
            {
                SerializeInternal(item, ms);
            }
        }

        private void WriteEnum(Stream ms, Enum value)
        {
            var enumType = Enum.GetUnderlyingType(value.GetType());
            switch (enumType)
            {
                case {} when enumType == typeof(byte): ms.WriteByte(Convert.ToByte(value)); break;
                case {} when enumType == typeof(sbyte): ms.WriteByte((byte)Convert.ToSByte(value)); break;
                case {} when enumType == typeof(short): WriteShort(ms, Convert.ToInt16(value)); break;
                case {} when enumType == typeof(ushort): WriteShort(ms, (short)Convert.ToUInt16(value)); break;
                case {} when enumType == typeof(int): WriteInt(ms, Convert.ToInt32(value)); break;
                case {} when enumType == typeof(uint): WriteInt(ms, (int)Convert.ToUInt32(value)); break;
                case {} when enumType == typeof(long): WriteLong(ms, Convert.ToInt64(value)); break;
                case {} when enumType == typeof(ulong): WriteLong(ms, (long)Convert.ToUInt64(value)); break;
                
                default: throw new ArgumentNullException($"Unexpected underlying enum type {enumType}");
            };
        }

        private void WriteObject(Stream ms, Type t, object value)
        {
            var members = GetSerializableMembers(t);

            foreach (var member in members)
            {
                try
                {
                    var memberValue = member.Match(f => f.GetValue(value), p => p.GetValue(value));

                    var (converter, param) = GetConverter(member);
                    if (converter != null)
                    {
                        converter.Serialize(ms, memberValue, this, param);
                        continue;
                    }

                    SerializeInternal(memberValue, ms);
                }
                catch (Exception ex)
                {
                    var memberName = member.Match(f => f.Name, p => p.Name);
                    throw new SerializationException($"Failed to serialize {memberName} of {t.FullName}", ex);
                }

            }
        }

        private (IBinaryConverter Converter, object[] Param) GetConverter(OneOf<FieldInfo, PropertyInfo> member)
        {
            var attributes = member.Match(f => f.GetCustomAttributes(),
                p => p.GetCustomAttributes());
            if (attributes.FirstOrDefault(a => a is ConverterAttribute) is ConverterAttribute converterType)
            {
                var converter = GetConverter(converterType.SerializeConverterType);

                return (converter, converterType.SerializeParameters);
            }

            return (null, null);
        }

        public IBinaryConverter GetConverter(Type converterType)
        {
            if (!_convertersCache.TryGetValue(converterType, out var converter))
            {
                converter = (IBinaryConverter) CreateObject(converterType);
                _convertersCache[converterType] = converter;
            }

            return converter;
        }

        private (IBinaryConverter Converter, object Param) GetBackConverter(OneOf<FieldInfo, PropertyInfo> member)
        {
            var converterType = member.Match(f => f.GetCustomAttribute<ConverterAttribute>(),
                p => p.GetCustomAttribute<ConverterAttribute>());
            if (converterType != null)
            {
                var converter = GetConverter(converterType.DeserializeConverterType);
                return (converter, converterType.DeserializeParameters);
            }

            return (null, null);
        }

        private ICollection<OneOf<FieldInfo, PropertyInfo>> GetSerializableMembers(Type t)
        {
            if (_serializableMembersCache.TryGetValue(t, out var cachedMembers))
            {
                return cachedMembers;
            }
            
            var serializableMembers = GetAttributedMembers<SerializeAttribute>(t);
            var members = serializableMembers
                .OrderBy(m => m.attribute.Order)
                .Select(m => m.memberInfo)
                .ToList();
            _serializableMembersCache[t] = members;
            return members;
        }

        private static IEnumerable<(OneOf<FieldInfo, PropertyInfo> memberInfo, TAttribute attribute)> GetAttributedMembers<TAttribute>(Type t) where TAttribute : Attribute
        {
            var properties = t
                .GetProperties()
                .Where(p => p.CanRead)
                .Select(p => (OneOf<FieldInfo, PropertyInfo>) p);
            var fields = t.GetFields()
                .Select(f => (OneOf<FieldInfo, PropertyInfo>) f);
            var serializableMembers = fields
                .Concat(properties)
                .Select(memberInfo => (memberInfo,
                    attribute: memberInfo.Match(f => f.GetCustomAttribute<TAttribute>(),
                        p => p.GetCustomAttribute<TAttribute>())))
                .Where(m => m.attribute != null);
            return serializableMembers;
        }

        private void WriteShort(Stream ms, short s)
        {
            ms.WriteByte((byte)(s & 0xff));
            ms.WriteByte((byte)(s >> 8));
        }

        private void WriteInt(Stream ms, int i)
        {
            ms.WriteByte((byte)(i & 0xff));
            ms.WriteByte((byte)((i >> 8) & 0xff));
            ms.WriteByte((byte)((i >> 16) & 0xff));
            ms.WriteByte((byte)(i >> 24));
        }

        private void WriteLong(Stream ms, long i)
        {
            ms.WriteByte((byte)(i & 0xff));
            ms.WriteByte((byte)((i >> 8) & 0xff));
            ms.WriteByte((byte)((i >> 16) & 0xff));
            ms.WriteByte((byte)((i >> 24) & 0xff));
            ms.WriteByte((byte)((i >> 32) & 0xff));
            ms.WriteByte((byte)((i >> 40) & 0xff));
            ms.WriteByte((byte)((i >> 48) & 0xff));
            ms.WriteByte((byte)(i >> 56));
        }
    }
}