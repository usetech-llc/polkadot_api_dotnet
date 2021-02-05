using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts
{
    public class Balance
    {
        [Serialize(0)]
        [U128Converter]
        public BigInteger Value { get; set; }
    }
}