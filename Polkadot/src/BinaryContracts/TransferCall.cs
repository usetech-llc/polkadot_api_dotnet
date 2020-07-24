using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts
{
    public class TransferCall : IExtrinsicCall
    {
        [Serialize(0)]
        [FixedSizeArrayConverter(Size = 32)]
        public byte[] DestinationPublicKey { get; set; }
        [Serialize(1)]
        [Converter(ConverterType = typeof(CompactBigIntegerConverter))]
        public BigInteger Amount { get; set; }

        public TransferCall()
        {
        }

        public TransferCall(byte[] destinationPublicKey, BigInteger amount)
        {
            DestinationPublicKey = destinationPublicKey;
            Amount = amount;
        }
    }
}