
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class ExtrinsicMultiSignature : IExtrinsicSignature
    {
        [Serialize(0)]
        public SignatureType SignatureType { get; set; }
        [Serialize(1)]
        public byte[] Signature { get; set; }

        public ExtrinsicMultiSignature()
        {
        }

        public ExtrinsicMultiSignature(SignatureType signatureType, byte[] signature)
        {
            SignatureType = signatureType;
            Signature = signature;
        }
    }
}