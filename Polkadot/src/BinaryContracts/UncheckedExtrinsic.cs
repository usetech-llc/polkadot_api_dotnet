
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts
{
    public sealed class UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall>
        where TAddress : IExtrinsicAddress
        where TSignature : IExtrinsicSignature
        where TSignedExtra : IExtrinsicExtra
        where TCall : IExtrinsicCall
    {
        [Serialize(0)]
        public byte SignatureVersion { get; set; }
        [Serialize(1)]
        public TAddress Address { get; set; }
        [Serialize(2)]
        public TSignature Signature { get; set; }
        [Serialize(3)]
        public TSignedExtra Extra { get; set; }
        [Serialize(4)]
        public TCall Call { get; set; }

        public UncheckedExtrinsic()
        {
        }

        public UncheckedExtrinsic(bool signed, TAddress address, TSignature signature, TSignedExtra extra, TCall call)
        {
            //4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned. 
            SignatureVersion = (byte) (4 | (signed ? 0x80 : 0));
            Address = address;
            Signature = signature;
            Extra = extra;
            Call = call;
        }
    }
}