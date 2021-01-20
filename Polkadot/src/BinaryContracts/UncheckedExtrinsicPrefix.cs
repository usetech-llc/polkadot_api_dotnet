using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class UncheckedExtrinsicPrefix<TAddress, TSignature, TSignedExtra>
        where TAddress : IExtrinsicAddress
        where TSignature : IExtrinsicSignature
        where TSignedExtra : IExtrinsicExtra
    {
        [Serialize(0)]
        public TAddress Address { get; set; }
        [Serialize(1)]
        public TSignature Signature { get; set; }
        [Serialize(2)]
        public TSignedExtra Extra { get; set; }

        public UncheckedExtrinsicPrefix()
        {
        }

        public UncheckedExtrinsicPrefix(TAddress address, TSignature signature, TSignedExtra extra)
        {
            Address = address;
            Signature = signature;
            Extra = extra;
        }
    }
}