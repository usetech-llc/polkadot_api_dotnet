using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Signatures
{
    public class Sr25519
    {
        [Serialize(0)]
        [FixedSizeArrayConverter(64)]
        public byte[] Signature { get; set; }

        public Sr25519()
        {
        }

        public Sr25519(byte[] signature)
        {
            Signature = signature;
        }
    }
}