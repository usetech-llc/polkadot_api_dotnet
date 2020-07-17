using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OneOf;

namespace Polkadot.BinarySerializer
{
    public class BinarySerializer: IBinarySerializer
    {
        private readonly Dictionary<Type, IBinaryConverter> _convertersCache = new Dictionary<Type, IBinaryConverter>();
        
        public byte[] Serialize<T>(T value)
        {
            using var ms = new MemoryStream();
            Serialize(value, ms);
            return ms.ToArray();
        }

        public void Serialize<T>(T value, Stream stream)
        {
            Serialize(typeof(T), value, stream);
        }

        public long ReadLong(IEnumerator<byte> input)
        {
            long value = 0;
            var offset = 0;
            for (int i = 0; i < 8 && input.MoveNext(); i++)
            {
                value |= input.Current << offset;
                offset += 8;
            }

            return value;
        }

        private void Serialize(Type t, object value, Stream ms)
        {
            if (typeof(IBinarySerializable).IsAssignableFrom(t))
            {
                ((IBinarySerializable)value).Serialize(ms, this);
                return;
            }

            if (typeof(IEnumerable).IsAssignableFrom(t))
            {
                var enumerable = (IEnumerable) value;
                foreach (var item in enumerable)
                {
                    Serialize(item.GetType(), item, ms);
                }
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
                
                default:
                    WriteObject(ms, t, value);
                    break;
            }
        }

        private void WriteEnum(Stream ms, Enum value)
        {
            var enumType = Enum.GetUnderlyingType(value.GetType());
            if (enumType == typeof(byte))
            {
                ms.WriteByte(Convert.ToByte(value));
                return;
            }

            if (enumType == typeof(short))
            {
                WriteShort(ms, Convert.ToInt16(value));
                return;
            }

            if (enumType == typeof(int))
            {
                WriteInt(ms, Convert.ToInt32(value));
                return;
            }
            
            if (enumType == typeof(long))
            {
                WriteLong(ms, Convert.ToInt64(value));
            }
        }

        private void WriteObject(Stream ms, Type t, object value)
        {
            var properties = t
                .GetProperties()
                .Where(p => p.CanRead)
                .Select(p => (OneOf<FieldInfo, PropertyInfo>) p);
            var fields = t.GetFields()
                .Select(f => (OneOf<FieldInfo, PropertyInfo>) f);
            var members = fields
                .Concat(properties)
                .Select(memberInfo => (memberInfo,
                    attribute: memberInfo.Match(f => f.GetCustomAttribute<SerializeAttribute>(),
                        p => p.GetCustomAttribute<SerializeAttribute>())))
                .Where(m => m.attribute != null)
                .OrderBy(m => m.attribute.Order)
                .Select(m => m.memberInfo);

            foreach (var member in members)
            {
                var memberValue = member.Match(f => f.GetValue(value), p => p.GetValue(value));
                var converterType = member.Match(f => f.GetCustomAttribute<ConverterAttribute>(), p => p.GetCustomAttribute<ConverterAttribute>());
                if (converterType != null)
                {
                    if (!_convertersCache.TryGetValue(converterType.ConverterType, out var converter))
                    {
                        converter = (IBinaryConverter)Activator.CreateInstance(converterType.ConverterType);
                        _convertersCache[converterType.ConverterType] = converter;
                    }
                    
                    converter.Serialize(ms, memberValue, this);
                    continue;
                }
                
                var type = member.Match(f => f.FieldType, p => p.PropertyType);
                Serialize(type, memberValue, ms);
            }
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