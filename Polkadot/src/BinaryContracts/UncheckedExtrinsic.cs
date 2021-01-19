
using System.IO;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.BinaryContracts
{
    public sealed class UncheckedExtrinsic<TAddress, TSignature, TSignedExtra, TCall> : IBinarySerializable
        where TAddress : IExtrinsicAddress
        where TSignature : IExtrinsicSignature
        where TSignedExtra : IExtrinsicExtra
        where TCall : IExtrinsicCall
    {
        private const byte TransactionVersion = 4;
        
        [Serialize(0)]
        public Option<UncheckedExtrinsicPrefix<TAddress, TSignature, TSignedExtra>> Prefix { get; set; }
        
        [Serialize(1)]
        public TCall Call { get; set; }

        public UncheckedExtrinsic()
        {
        }

        public UncheckedExtrinsic(TAddress address, TSignature signature, TSignedExtra extra, TCall call)
        {
            Prefix = new Option<UncheckedExtrinsicPrefix<TAddress, TSignature, TSignedExtra>>(
                new UncheckedExtrinsicPrefix<TAddress, TSignature, TSignedExtra>(address, signature, extra));
            Call = call;
        }

        public void Serialize(Stream stream, IBinarySerializer serializer)
        {
            Prefix.Value.Switch(
                _ => serializer.Serialize((byte)(TransactionVersion & 0b0111_1111), stream),
                prefix =>
                {
                    serializer.Serialize((byte)(TransactionVersion | 0b1000_0000), stream);
                    serializer.Serialize(prefix, stream);
                });

            serializer.Serialize(Call, stream);
        }
    }
}