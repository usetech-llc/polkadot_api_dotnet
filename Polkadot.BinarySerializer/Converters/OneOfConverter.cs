using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OneOf;
using Polkadot.BinarySerializer.Extensions;

namespace Polkadot.BinarySerializer.Converters
{
    public class OneOfConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            var innerValue = ((IOneOf) value).Value;
            var index = (int) value.GetType().GetField("_index", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!.GetValue(value);
            stream.WriteByte((byte)index);
            serializer.Serialize(innerValue);
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var index = stream.ReadByteThrowIfStreamEnd();
            var innerType = type.GetGenericArguments()[index];
            var innerValue = deserializer.Deserialize(innerType, stream);
            var cast = type.GetMethod("op_Implicit", new[] {innerType});
            return cast!.Invoke(null, new[] {innerValue});
        }
    }
}