using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using Polkadot.BinarySerializer;

namespace Polkadot.BinarySerializer.Converters
{
    public abstract class BaseArrayConverter : IBinaryConverter
    {

        public abstract void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] param);

        public abstract object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] param);

        public void SerializeArray(Stream stream, object value, IBinarySerializer serializer, object[] param)
        {
            if (value == null)
            {
                return;
            }
            
            var itemConverterType = param?[0] as Type;
            IBinaryConverter converter = null;
            if (itemConverterType != null)
            {
                converter = serializer.GetConverter(itemConverterType);
            }

            var enumerable = (IEnumerable)value;
            foreach (var v in enumerable)
            {
                if (converter != null)
                {
                    converter.Serialize(stream, v, serializer, param?[1] as object[]);
                }
                else
                {
                    serializer.Serialize(v, stream);
                }
            }
        }

        public object DeserializeArray(Type type, Stream stream, IBinarySerializer deserializer, object[] param, int size)
        {
            var elementType = type.GetElementType();
            if (elementType == null)
            {
                throw new ArgumentException($"Type {type.FullName} is not an array, unable to convert.");
            }

            if (elementType == typeof(byte))
            {
                var byteArray = new byte[size];
                stream.Read(byteArray, 0, size);
                return byteArray;
            }

            var array = Array.CreateInstance(elementType, size);
            var itemConverterType = param?[0] as Type;
            IBinaryConverter converter = null;
            if (itemConverterType != null)
            {
                converter = deserializer.GetConverter(itemConverterType);
            }

            for (int i = 0; i < size; i++)
            {
                var item = converter == null
                    ? deserializer.Deserialize(elementType, stream)
                    : converter.Deserialize(elementType, stream, deserializer, param?[1] as object[]);
                array.SetValue(item, i);
            }

            return array;
        }
    }
}