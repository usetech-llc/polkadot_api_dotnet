using System;
using System.Collections.Generic;
using System.Linq;
using Blake2Core;
using OneOf;
using Polkadot.BinaryContracts;
using Polkadot.BinaryContracts.Extrinsic;
using Polkadot.BinaryContracts.Signatures;
using Polkadot.BinarySerializer;
using Polkadot.Data;
using Polkadot.Utils;
using Schnorrkel;

namespace Polkadot.Api
{
    public class Signer : ISigner
    {
        public IApplication Application;

        public Signer(IApplication application)
        {
            Application = application;
        }

        public IExtrinsicSignature Sign(byte[] publicKey, byte[] privateKey, byte[] message)
        {
            return new ExtrinsicMultiSignature(new Sr25519(Sr25519v091.SignSimple(publicKey, privateKey, message)));
        }

        public bool VerifySignature(byte[] sign, byte[] publicKey, byte[] message)
        {
            return Sr25519v091.Verify(sign, publicKey, message);
        }

        public byte[] GetSignaturePayload<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> extrinsic) 
            where TAddress : IExtrinsicAddress 
            where TSignature : IExtrinsicSignature 
            where TSignedExtra : IExtrinsicExtra 
            where TCall : IExtrinsicCall
        {
            return extrinsic.Prefix.Value.Match(
                _ => Array.Empty<byte>(),
                prefix =>
                {
                    var metadata = Application.GetProtocolParameters().Metadata;
                    var extrinsicExtension = metadata.RawMetadata.GetExtrinsicExtension();
                    if (extrinsicExtension == null)
                    {
                        throw new NotSupportedException(
                            $"Metadata with version {metadata.MetadataVersion} doesn't have information about signature payload construction. Try use {nameof(SignaturePayload)} instead or override {nameof(ISigner)}.{nameof(GetSignaturePayload)} method.");
                    }

                    var additionalSignedTable =
                        AdditionalSignersUncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall>(extrinsic.BlockHeader);

                    var extraSigned = extrinsicExtension.Select(x =>
                    {
                        if (!additionalSignedTable.TryGetValue(x, out var additionalSigned))
                        {
                            throw new NotSupportedException(
                                $"Unknown extrinsic extension {x}, unable to make signature payload for it.");
                        }

                        return additionalSigned(extrinsic);
                    });

                    var signaturePayload = SignaturePayload.Create(extrinsic.Call, prefix.Extra, extraSigned);
                    var serialized = Application.Serializer.Serialize(signaturePayload);
                    //Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
                    if (serialized.Length > 256)
                    {
                        serialized = Blake2B.ComputeHash(serialized);
                    }

                    return serialized;
                });
        }

        public Dictionary<string, Func<UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall>, object>>
            AdditionalSignersUncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall>(BlockHeader blockHeader)
            where TAddress : IExtrinsicAddress
            where TSignature : IExtrinsicSignature
            where TSignedExtra : IExtrinsicExtra
            where TCall : IExtrinsicCall
        {
            var blockHash = Application.GetBlockHash(new GetBlockHashParams(){ BlockNumber = blockHeader.Number}).Hash;
            var runtimeVersion = Application.GetRuntimeVersion(new GetRuntimeVersionParams()
                {BlockHash = blockHash});
            return new Dictionary<string, Func<UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall>, object>>()
            {
                { "CheckSpecVersion", p => CheckSpecVersion(p, runtimeVersion) },
                { "CheckTxVersion", p => CheckTxVersion(p, runtimeVersion) },
                { "CheckGenesis", CheckGenesis },
                { "CheckEra", CheckEra },
                { "CheckNonce", CheckNonce },
                { "CheckWeight", CheckWeight },
                { "ChargeTransactionPayment", ChargeTransactionPayment},
                { "CheckMortality", CheckMortality}
            };
        }

        private object CheckMortality<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            var era = arg.Prefix.Value.AsT1.Extra.GetEraIfAny();
            var current = arg.BlockHeader.Number;
            var n = era.Birth(current);
            var hash = Application.GetBlockHash(new GetBlockHashParams() {BlockNumber = n}).Hash;
            return hash.HexToByteArray();
        }

        private object ChargeTransactionPayment<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            return Empty.Instance;
        }

        private object CheckWeight<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            return Empty.Instance;
        }

        private object CheckNonce<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            return Empty.Instance;
        }

        private object CheckEra<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            return arg.Prefix.Value.AsT1.Extra.GetEraIfAny().Value.Match(immortal => Application.GetProtocolParameters().GenesisBlockHash, MortalBlockHash);
        }

        private byte[] MortalBlockHash(MortalEra mortal)
        {
            return Application.GetBlockHash(new GetBlockHashParams()
            {
                BlockNumber = mortal.Phase
            }).Hash.HexToByteArray();
        }

        private object CheckGenesis<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            return Application.GetProtocolParameters().GenesisBlockHash;
        }

        private object CheckTxVersion<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg, RuntimeVersion runtimeVersion) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            return runtimeVersion.TransactionVersion;
        }

        private object CheckSpecVersion<TAddress, TSignature, TSignedExtra, TCall>(UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> arg, RuntimeVersion runtimeVersion) where TAddress : IExtrinsicAddress where TSignature : IExtrinsicSignature where TSignedExtra : IExtrinsicExtra where TCall : IExtrinsicCall
        {
            return runtimeVersion.SpecVersion;
        }
    }
}