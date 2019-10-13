namespace Schnorrkel.Signed
{
    using Schnorrkel.Ristretto;
    using Schnorrkel.Scalars;
    using System;

    public struct Signature
    {
        public CompressedRistretto R { get; set; }
        public Scalar S { get; set; }

        public byte[] ToBytes()
        {
            var bytes = new byte[Consts.SIGNATURE_LENGTH];
            R.ToBytes().AsMemory().CopyTo(bytes.AsMemory(0, 32));
            S.ScalarBytes.AsMemory().CopyTo(bytes.AsMemory(32, 32));
            bytes[63] |= 128;
            return bytes;
        }
    }
}
