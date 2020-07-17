
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class ExtrinsicAddress : IExtrinsicAddress
    {
        [Serialize(0)]
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