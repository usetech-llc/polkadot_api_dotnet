using System.IO;
using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace Polkadot.BinaryContracts
{
    public class CompactBigIntegerConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer)
        {
            var encoded = Scale.EncodeCompactInteger((BigInteger)value);
            stream.Write(encoded.Bytes);
        }
    }
}