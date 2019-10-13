namespace Schnorrkel.Scalars
{
    using System;

    public class Scalar
    {
        /// `bytes` is a little-endian byte encoding of an integer representing a scalar modulo the
        /// group order.
        public byte[] ScalarBytes { get; set; }
        public Scalar52 ScalarInner { get; private set; }

        //public Scalar(byte[] scalarBytes)
        //{
        //    _scalarBytes = scalarBytes;
        //}

        public byte[] GetBytes()
        {
            return ScalarBytes;
        }

        public void Recalc()
        {
            ScalarInner = new Scalar52(ScalarBytes);
        }

        public sbyte[] ToRadix16()
        {
            var output = new sbyte[64];

            // Step 1: change radix.
            // Convert from radix 256 (bytes) to radix 16 (nibbles)
            var botHalf = new Func<byte, byte>(x => { return (byte)((x >> 0) & 15); });
            var topHalf = new Func<byte, byte>(x => { return (byte)((x >> 4) & 15); });

            for (var i = 0; i < 32; i++)
            {
                output[2 * i] = (sbyte)botHalf(ScalarBytes[i]);
                output[2 * i + 1] = (sbyte)topHalf(ScalarBytes[i]);
            }
            // Precondition note: since self[31] <= 127, output[63] <= 7

            // Step 2: recenter coefficients from [0,16) to [-8,8)
            for (var i = 0; i < 63; i++)
            {
                sbyte carry = (sbyte)((output[i] + 8) >> 4);
                output[i] -= (sbyte)(carry << 4);
                output[i + 1] += (sbyte)carry;
            }
            // Precondition note: output[63] is not recentered.  It
            // increases by carry <= 1.  Thus output[63] <= 8.

            return output;
        }

        public static Scalar FromBytesModOrder(byte[] data)
        {
            var sc = UnpackedScalar.FromBytes(data);
            return UnpackedScalar.Pack(sc);
        }

        public static Scalar FromBytesModOrderWide(byte[] data)
        {
            var sc = UnpackedScalar.FromBytesWide(data);
            return UnpackedScalar.Pack(sc);
        }
    }
}
