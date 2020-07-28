using System;
using System.IO;
using Polkadot.BinarySerializer;

namespace Polkadot.BinarySerializer.Converters
{
    public class PrefixedArrayConverter : BaseArrayConverter
    {
        public override void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] param)
        {
            var length = ((Array) value).Length;
            var compactLength = Scale.EncodeCompactInteger(length).Bytes;
            stream.Write(compactLength, 0, compactLength.Length);
            serializer.Serialize(value, stream);
        }

        public override object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] param)
        {
            var length = (int) Scale.DecodeCompactInteger(stream).Value;
            return DeserializeArray(type, stream, deserializer, param, length);
        }
    }
}