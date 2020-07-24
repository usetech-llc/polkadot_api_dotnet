using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace Polkadot.DataStructs
{
    using Polkadot.Api;
    using Polkadot.Source.Utils;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Text;

    public class SignaturePayload
    {
        public BigInteger Nonce { get; set; }
        public int MethodBytesLength { get; set; }
        public byte[] MethodBytes { get; set; }
        public ExtrinsicEra Era { get; set; }
        public byte[] AuthoringBlockHash { get; set; }

        public long SerializeBinary(ref byte[] buf)
        {
            long writtenLength = 0;

            // Nonce
            var compactNonce = Scale.EncodeCompactInteger(Nonce);
            writtenLength += Scale.WriteCompactToBuf(compactNonce, ref buf, writtenLength);

            // Method
            MethodBytes.AsMemory().Slice(0, MethodBytesLength).CopyTo(buf.AsMemory((int)writtenLength));
            writtenLength += MethodBytesLength;

            // Extrinsic Era
            buf[writtenLength++] = (byte)Era;

            // Authoring Block Hash
            AuthoringBlockHash.CopyTo(buf.AsMemory((int)writtenLength));
            writtenLength += Consts.BLOCK_HASH_SIZE;

            return writtenLength;
        }
    }
}
