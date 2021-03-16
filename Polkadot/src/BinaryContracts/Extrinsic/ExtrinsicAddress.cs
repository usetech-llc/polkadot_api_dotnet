using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Extrinsic
{
    public class ExtrinsicAddress : IExtrinsicAddress
    {
        [Serialize(0)]
        public PublicKey PublicKey { get; set; }

        public ExtrinsicAddress()
        {
        }

        public ExtrinsicAddress(PublicKey publicKey)
        {
            PublicKey = publicKey;
        }
    }
}