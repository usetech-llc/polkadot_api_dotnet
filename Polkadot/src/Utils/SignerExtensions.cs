using Polkadot.Api;
using Polkadot.BinaryContracts;
using Polkadot.BinaryContracts.Extrinsic;
using Polkadot.BinarySerializer;

namespace Polkadot.Utils
{
    public static class SignerExtensions
    {
        public static void SignUncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall>(
            this ISigner signer,
            UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> extrinsic,
            byte[] publicKey,
            byte[] privateKey)
                where TAddress : IExtrinsicAddress
                where TSignature : IExtrinsicSignature
                where TSignedExtra : IExtrinsicExtra
                where TCall : IExtrinsicCall
        {
            extrinsic.Prefix.Value.Switch(
                _ => { },
                prefix =>
                {
                    var payload = signer.GetSignaturePayload(extrinsic);
                    var sign = signer.Sign(publicKey, privateKey, payload);
                    prefix.Signature = (TSignature) sign;
                });
        }
    }
}