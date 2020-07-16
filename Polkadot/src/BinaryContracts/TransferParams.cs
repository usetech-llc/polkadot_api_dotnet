using System.Numerics;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class TransferParams
    {
        [Serialize(0)]
        public byte[] DestinationPublicKey { get; set; }
        [Serialize(1)]
        [Converter(typeof(CompactBigIntegerConverter))]
        public BigInteger Amount { get; set; }

        public TransferParams()
        {
        }

        public TransferParams(byte[] destinationPublicKey, BigInteger amount)
        {
            DestinationPublicKey = destinationPublicKey;
            Amount = amount;
        }
    }
}