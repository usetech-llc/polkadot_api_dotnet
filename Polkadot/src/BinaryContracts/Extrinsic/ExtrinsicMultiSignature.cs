using OneOf;
using Polkadot.BinaryContracts.Signatures;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Extrinsic
{
    public class ExtrinsicMultiSignature : IExtrinsicSignature
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Ed25519, Sr25519, Ecdsa> MultiSignature { get; set; }

        public ExtrinsicMultiSignature()
        {
        }

        public ExtrinsicMultiSignature(Ed25519 multiSignature)
        {
            MultiSignature = multiSignature;
        }
        public ExtrinsicMultiSignature(Sr25519 multiSignature)
        {
            MultiSignature = multiSignature;
        }
        public ExtrinsicMultiSignature(Ecdsa multiSignature)
        {
            MultiSignature = multiSignature;
        }
    }
}