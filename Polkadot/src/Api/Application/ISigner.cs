using Polkadot.BinaryContracts;
using Polkadot.BinaryContracts.Extrinsic;
using Polkadot.BinarySerializer;

namespace Polkadot.Api
{
    public interface ISigner
    {
        IExtrinsicSignature Sign(byte[] publicKey, byte[] privateKey, byte[] message);

        bool VerifySignature(byte[] sign, byte[] publicKey, byte[] message);

        byte[] GetSignaturePayload<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> extrinsic)
            where TAddress : IExtrinsicAddress
            where TSignature : IExtrinsicSignature
            where TSignedExtra : IExtrinsicExtra
            where TCall : IExtrinsicCall;
    }
}