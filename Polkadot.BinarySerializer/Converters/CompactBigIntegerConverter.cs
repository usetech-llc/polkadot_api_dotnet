using System;
using System.IO;
using System.Numerics;
using Polkadot.BinarySerializer;

namespace Polkadot.BinarySerializer.Converters
{
    public class CompactBigIntegerConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] param)
        {
            var encoded = Scale.EncodeCompactInteger((BigInteger)value);
            stream.Write(encoded.Bytes, 0, encoded.Bytes.Length);
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] param)
        {
            return Scale.DecodeCompactInteger(stream).Value;
        }
    }
}