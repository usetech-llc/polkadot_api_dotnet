using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts
{
    public class ExtrinsicAddress : IExtrinsicAddress
    {
        [Serialize(0)]
        [FixedSizeArrayConverter(Size = 32)]
        public byte[] PublicKey { get; set; }

        public ExtrinsicAddress()
        {
        }

        public ExtrinsicAddress(byte[] publicKey)
        {
            PublicKey = publicKey;
        }
    }
}