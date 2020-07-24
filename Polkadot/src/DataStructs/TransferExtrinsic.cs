using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace Polkadot.DataStructs
{
    using Polkadot.Api;
    using Polkadot.Source.Utils;
    using System;
    using System.Numerics;

    public class Method
    {
        public byte ModuleIndex { get; set; }
        public byte MethodIndex { get; set; }
    };

    public class TransferMethod : Method {
        public byte[] ReceiverPublicKey { get; set; }
        public BigInteger Amount;
    }

    public class TransferExtrinsic : Extrinsic
    {
        public TransferMethod Method { get; set; }

        public TransferExtrinsic()
        {
            Method = new TransferMethod();
            Signature = new Signature();
        }

        public long SerializeMethodBinary(ref byte[] buf, long offset = 0)
        {
            long writtenLength = 0;

            // Module + Method
            buf[offset + writtenLength++] = Method.ModuleIndex;
            buf[offset + writtenLength++] = Method.MethodIndex;

            // Address separator
            buf[offset + writtenLength++] = Consts.ADDRESS_SEPARATOR;

            // Receiving address public key
            Method.ReceiverPublicKey.AsMemory().CopyTo(buf.AsMemory((int)(offset + writtenLength)));
            writtenLength += Consts.SR25519_PUBLIC_SIZE;

            // Compact-encode amount
            var compactAmount = Scale.EncodeCompactInteger(Method.Amount);

            // Amount
            writtenLength += Scale.WriteCompactToBuf(compactAmount, ref buf, offset + writtenLength);

            return writtenLength;
        }

        public long SerializeBinary(ref byte[] buf)
        {
            // Compact-encode amount
            var compactAmount = Scale.EncodeCompactInteger(Method.Amount);

            // Compact-encode nonce
            var compactNonce = Scale.EncodeCompactInteger(Signature.Nonce);

            // Calculate total extrinsic length
            Length = 134 + compactAmount.Length + compactNonce.Length;
            var compactLength = Scale.EncodeCompactInteger(Length);

            /////////////////////////////////////////
            // Serialize and write to buffer

            long writtenLength = 0;

            // Length
            writtenLength += Scale.WriteCompactToBuf(compactLength, ref buf, writtenLength);
            
            // Signature version
            buf[writtenLength++] = Signature.Version;

            // Address separator
            buf[writtenLength++] = Consts.ADDRESS_SEPARATOR;

            // Signer public key
            Signature.SignerPublicKey.AsMemory().CopyTo(buf.AsMemory((int)writtenLength));
            writtenLength += Consts.SR25519_PUBLIC_SIZE;

            // SR25519 Signature
            Signature.Sr25519Signature.AsMemory().CopyTo(buf.AsMemory((int)writtenLength));
            writtenLength += Consts.SR25519_SIGNATURE_SIZE;

            // Nonce
            writtenLength += Scale.WriteCompactToBuf(compactNonce, ref buf, writtenLength);

            // Extrinsic Era
            buf[writtenLength++] = (byte)Signature.Era;

            // Method
            writtenLength += SerializeMethodBinary(ref buf, writtenLength);

            return writtenLength;
        }
    }
}
