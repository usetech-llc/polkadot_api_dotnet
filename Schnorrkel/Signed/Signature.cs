namespace Schnorrkel.Signed
{
    using Schnorrkel.Ristretto;
    using Schnorrkel.Scalars;
    using System;

    public struct Signature
    {
        public CompressedRistretto R { get; set; }
        public Scalar S { get; set; }

        public void FromBytes(byte[] signatureBytes)
        {
            var r = new CompressedRistretto(signatureBytes.AsMemory(0, 32).ToArray());
            var s = new Scalar();
            s.ScalarBytes = new byte[32];
            signatureBytes.AsMemory(32, 32).CopyTo(s.ScalarBytes);
            s.Recalc();

            R = r;
            S = s;
        }

        public byte[] ToBytes011()
        {
            var bytes = new byte[Consts.SIGNATURE_LENGTH];
            R.ToBytes().AsMemory().CopyTo(bytes.AsMemory(0, 32));
            S.ScalarBytes.AsMemory().CopyTo(bytes.AsMemory(32, 32));
            return bytes;
        }

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
