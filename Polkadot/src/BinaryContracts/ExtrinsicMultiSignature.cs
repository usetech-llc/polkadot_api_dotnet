
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts
{
    public class ExtrinsicMultiSignature : IExtrinsicSignature
    {
        [Serialize(0)]
        public SignatureType SignatureType { get; set; }
        [Serialize(1)]
        [FixedSizeArrayConverter(Size = 64)]
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