using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts
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