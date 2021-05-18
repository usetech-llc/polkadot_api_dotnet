using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using OneOf;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.BinarySerializer.Types;

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

        private readonly Dictionary<Type, Func<Type, Stream, object>> _deserializers;
        private readonly Dictionary<Type, Action<Stream, object>> _serializers;

        public BinarySerializer()
        {
            _deserializers = new ()
            {
                { typeof(byte), (_, stream) => stream.ReadByteThrowIfStreamEnd() },
                { typeof(sbyte), (_, stream) => (sbyte)stream.ReadByteThrowIfStreamEnd() },
                { typeof(short), (_, stream) => ReadShort(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()) },
                { typeof(ushort), (_, stream) => (ushort)ReadShort(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()) },
                { typeof(int), (_, stream) => ReadInt(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()) },
                { typeof(uint), (_, stream) => (uint)ReadInt(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()) },
                { typeof(long), (_, stream) => ReadLong(stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd()) },
                { typeof(ulong), (_, stream) => ReadUlong(stream) },
                { typeof(Int128), (_, stream) => ReadInt128(stream) },
                { typeof(UInt128), (_, stream) => ReadUInt128(stream) },
                { typeof(Int256), (_, stream) => ReadInt256(stream) },
                { typeof(UInt256), (_, stream) => ReadUInt256(stream) },
                { typeof(Int512), (_, stream) => ReadInt512(stream) },
                { typeof(UInt512), (_, stream) => ReadUInt512(stream) },
                { typeof(bool), (_, stream) => ReadBool(stream.ReadByteThrowIfStreamEnd()) },
                { typeof(Tuple<>), ReadTuple },
                { typeof(Tuple<,>), ReadTuple },
                { typeof(Tuple<,,>), ReadTuple },
                { typeof(Tuple<,,,>), ReadTuple },
                { typeof(Tuple<,,,,>), ReadTuple },
                { typeof(Tuple<,,,,,>), ReadTuple },
                { typeof(Tuple<,,,,,,>), ReadTuple },
                { typeof(Tuple<,,,,,,,>), ReadTuple },
                { typeof(ValueTuple<>), ReadTuple },
                { typeof(ValueTuple<,>), ReadTuple },
                { typeof(ValueTuple<,,>), ReadTuple },
                { typeof(ValueTuple<,,,>), ReadTuple },
                { typeof(ValueTuple<,,,,>), ReadTuple },
                { typeof(ValueTuple<,,,,,>), ReadTuple },
                { typeof(ValueTuple<,,,,,,>), ReadTuple },
                { typeof(ValueTuple<,,,,,,,>), ReadTuple },
            };

            _serializers = new()
            {
                {typeof(byte), (stream, value) => stream.WriteByte((byte)value)},
                {typeof(sbyte), (stream, value) => stream.WriteByte((byte)(sbyte)value)},
                { typeof(short), (stream, value) => WriteShort(stream, (short)value) },
                { typeof(ushort), (stream, value) => WriteShort(stream, (short)(ushort)value) },
                { typeof(int), (stream, value) => WriteInt(stream, (int)value) },
                { typeof(uint), (stream, value) => WriteInt(stream, (int)(uint)value) },
                { typeof(long), (stream, value) => WriteLong(stream, (long)value) },
                { typeof(ulong), (stream, value) => WriteLong(stream, (long)(ulong)value) },
                { typeof(Int128), (stream, value) => WriteInt128(stream, (Int128)value) },
                { typeof(UInt128), (stream, value) => WriteUint128(stream, (UInt128)value) },
                { typeof(Int256), (stream, value) => WriteInt256(stream, (Int256)value) },
                { typeof(UInt256), (stream, value) => WriteUint256(stream, (UInt256)value) },
                { typeof(Int512), (stream, value) => WriteInt512(stream, (Int512)value) },
                { typeof(UInt512), (stream, value) => WriteUint512(stream, (UInt512)value) },
                { typeof(Enum), (stream, value) => WriteEnum(stream, (Enum)value) },
                { typeof(bool), (stream, value) => WriteBool(stream, (bool)value) },
                { typeof(string), (stream, value) => WriteString(stream, (string)value) },
                { typeof(Tuple<>), WriteTuple },
                { typeof(Tuple<,>), WriteTuple },
                { typeof(Tuple<,,>), WriteTuple },
                { typeof(Tuple<,,,>), WriteTuple },
                { typeof(Tuple<,,,,>), WriteTuple },
                { typeof(Tuple<,,,,,>), WriteTuple },
                { typeof(Tuple<,,,,,,>), WriteTuple },
                { typeof(Tuple<,,,,,,,>), WriteTuple },
                { typeof(ValueTuple<>), WriteTuple },
                { typeof(ValueTuple<,>), WriteTuple },
                { typeof(ValueTuple<,,>), WriteTuple },
                { typeof(ValueTuple<,,,>), WriteTuple },
                { typeof(ValueTuple<,,,,>), WriteTuple },
                { typeof(ValueTuple<,,,,,>), WriteTuple },
                { typeof(ValueTuple<,,,,,,>), WriteTuple },
                { typeof(ValueTuple<,,,,,,,>), WriteTuple },
            };
        }

        public BinarySerializer(IndexResolver resolver, SerializerSettings settings) : this()
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

        public object Deserialize(Type type, byte[] stream)
        {
            using var ms = new MemoryStream(stream);
            return DeserializeInternal(type, ms);
        }

        private object DeserializeInternal(Type type, Stream stream)
        {
            if (typeof(IBinaryDeserializable).IsAssignableFrom(type))
            {
                var deserializable = (IBinaryDeserializable)CreateObject(type);
                return deserializable.Deserialize(stream, this);
            }
            
            if (_deserializers.TryGetValue(type, out var deserializer))
            {
                return deserializer(type, stream);
            }

            if (type.IsGenericType &&
                _deserializers.TryGetValue(type.GetGenericTypeDefinition(), out var genericDeserializer))
            {
                return genericDeserializer(type, stream);
            }

            if (type.IsEnum)
            {
                return ReadEnum(type, stream);
            }

            return ReadObject(type, stream);
        }

        private static bool ReadBool(byte b)
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

        public UInt512 ReadUInt512(Stream ms)
        {
            var ul0 = ReadUlong(ms);
            var ul1 = ReadUlong(ms);
            var ul2 = ReadUlong(ms);
            var ul3 = ReadUlong(ms);
            var ul4 = ReadUlong(ms);
            var ul5 = ReadUlong(ms);
            var ul6 = ReadUlong(ms);
            var ul7 = ReadUlong(ms);

            return new UInt512(ul0, ul1, ul2, ul3, ul4, ul5, ul6, ul7);
        }

        public Int512 ReadInt512(Stream ms)
        {
            var ul0 = ReadUlong(ms);
            var ul1 = ReadUlong(ms);
            var ul2 = ReadUlong(ms);
            var ul3 = ReadUlong(ms);
            var ul4 = ReadUlong(ms);
            var ul5 = ReadUlong(ms);
            var ul6 = ReadUlong(ms);
            var ul7 = ReadUlong(ms);

            return new Int512(ul0, ul1, ul2, ul3, ul4, ul5, ul6, ul7);
        }

        public Int256 ReadInt256(Stream ms)
        {
            var ul0 = ReadUlong(ms);
            var ul1 = ReadUlong(ms);
            var ul2 = ReadUlong(ms);
            var ul3 = ReadUlong(ms);

            return new Int256(ul0, ul1, ul2, ul3);
        }

        public UInt256 ReadUInt256(Stream ms)
        {
            var ul0 = ReadUlong(ms);
            var ul1 = ReadUlong(ms);
            var ul2 = ReadUlong(ms);
            var ul3 = ReadUlong(ms);

            return new UInt256(ul0, ul1, ul2, ul3);
        }

        private Int128 ReadInt128(Stream ms)
        {
            var ul0 = ReadUlong(ms);
            var ul1 = ReadUlong(ms);

            return new Int128(ul0, ul1);
        }

        private UInt128 ReadUInt128(Stream ms)
        {
            var ul0 = ReadUlong(ms);
            var ul1 = ReadUlong(ms);

            return new UInt128(ul0, ul1);
        }
        
        
        private ulong ReadUlong(Stream stream)
        {
            return (ulong) ReadLong(
                stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), 
                stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd(), stream.ReadByteThrowIfStreamEnd());
        }

        private static long ReadLong(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
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

        private static int ReadInt(byte b0, byte b1, byte b2, byte b3)
        {
            var ul0 = (ulong) b0;
            var ul1 = ((ulong) b1) << 8;
            var ul2 = ((ulong) b2) << 16;
            var ul3 = ((ulong) b3) << 24;

            return (int) (ul0 | ul1 | ul2 | ul3);
        }

        private static short ReadShort(byte b0, byte b1)
        {
            var ul0 = (ulong) b0;
            var ul1 = ((ulong) b1) << 8;
            return (short)(ul0 | ul1);
        }
        
        private object ReadTuple(Type type, Stream stream)
        {
            var values = type.GenericTypeArguments.Select(t => DeserializeInternal(t, stream)).ToArray();
    
            return type.GetMethod("Create")!.MakeGenericMethod(type.GenericTypeArguments).Invoke(null, values);
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
            
            if (_serializers.TryGetValue(type, out var serializer))
            {
                serializer(ms, value);
                return;
            }

            if (type.IsGenericType &&
                _serializers.TryGetValue(type.GetGenericTypeDefinition(), out var genericSerializer))
            {
                genericSerializer(ms, value);
                return;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                WriteEnumerable(ms, (IEnumerable)value);
                return;
            }
            
            WriteObject(ms, type, value);
        }

        private void WriteTuple(Stream stream, object value)
        {
            var type = value.GetType();

            var index = 1;
            foreach (var genericParameter in type.GenericTypeArguments)
            {
                var item = type.GetProperty($"Item{index}")!.GetValue(value);
                SerializeInternal(item, stream);
                index++;
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

        private void WriteEnumerable(Stream ms, IEnumerable enumerable)
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

        private void WriteInt128(Stream ms, Int128 i)
        {
            var (l0, l1) = i;
            WriteLong(ms, (long)l0);
            WriteLong(ms, (long)l1);
        }

        private void WriteUint128(Stream ms, UInt128 i)
        {
            var (l0, l1) = i;
            WriteLong(ms, (long)l0);
            WriteLong(ms, (long)l1);
        }

        private void WriteInt256(Stream ms, Int256 i)
        {
            var (l0, l1, l2, l3) = i;
            WriteLong(ms, (long)l0);
            WriteLong(ms, (long)l1);
            WriteLong(ms, (long)l2);
            WriteLong(ms, (long)l3);
        }

        private void WriteUint256(Stream ms, UInt256 i)
        {
            var (l0, l1, l2, l3) = i;
            WriteLong(ms, (long)l0);
            WriteLong(ms, (long)l1);
            WriteLong(ms, (long)l2);
            WriteLong(ms, (long)l3);
        }
 
        private void WriteInt512(Stream ms, Int512 i)
        {
            var (l0, l1, l2, l3, l4, l5, l6, l7) = i;
            WriteLong(ms, (long)l0);
            WriteLong(ms, (long)l1);
            WriteLong(ms, (long)l2);
            WriteLong(ms, (long)l3);
            WriteLong(ms, (long)l4);
            WriteLong(ms, (long)l5);
            WriteLong(ms, (long)l6);
            WriteLong(ms, (long)l7);
        }
 
        private void WriteUint512(Stream ms, UInt512 i)
        {
            var (l0, l1, l2, l3, l4, l5, l6, l7) = i;
            WriteLong(ms, (long)l0);
            WriteLong(ms, (long)l1);
            WriteLong(ms, (long)l2);
            WriteLong(ms, (long)l3);
            WriteLong(ms, (long)l4);
            WriteLong(ms, (long)l5);
            WriteLong(ms, (long)l6);
            WriteLong(ms, (long)l7);
        }
    }
}