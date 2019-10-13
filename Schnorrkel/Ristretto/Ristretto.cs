namespace Schnorrkel.Ristretto
{
    using Schnorrkel.Scalars;
    using System;

    public class CompressedRistretto
    {
        public byte[] _compressedRistrettoBytes { get; set; }

        public CompressedRistretto(byte[] data)
        {
            _compressedRistrettoBytes = data;
        }

        public byte[] ToBytes()
        {
            return _compressedRistrettoBytes;
        }

        public byte[] GetBytes()
        {
            return _compressedRistrettoBytes;
        }
    }

    internal class RistrettoBasepointTable
    {
        private EdwardsBasepointTable edwardsBasepointTable;

        public RistrettoBasepointTable()
        {
            edwardsBasepointTable = Consts.ED25519_BASEPOINT_TABLE_INNER;
        }

        public RistrettoPoint Mul(Scalar s)
        {
            var ep = edwardsBasepointTable.Mul(s);

            return new RistrettoPoint(ep);
        }
    }

    internal class RistrettoPoint
    {
        public EdwardsPoint Ep;

        public RistrettoPoint(EdwardsPoint ep)
        {
            Ep = ep;
        }

        /// Compress this point using the Ristretto encoding.
        public CompressedRistretto Compress()
        {
            var invsqrt = 
            new Func<FieldElement51, (bool, FieldElement51)>((el) => {
                return FieldElement51.SqrtRatioI(FieldElement51.One(), el);
            });

            var X = Ep.X;
            var Y = Ep.Y;
            var Z = Ep.Z;
            var T = Ep.T;

            var u1 = Z.Add(Y).Mul(Z.Sub(Y));
            var u2 = X.Mul(Y);

            // Ignore return value since this is always square
            var inv = invsqrt(u1.Mul(u2.Square()));
            var i1 = inv.Item2.Mul(u1);
            var i2 = inv.Item2.Mul(u2);
            var z_inv = i1.Mul(i2.Mul(T));
            var den_inv = i2;

            var iX = X.Mul(Consts.SQRT_M1);
            var iY = Y.Mul(Consts.SQRT_M1);
            var ristretto_magic = (Consts.INVSQRT_A_MINUS_D);
            var enchanted_denominator = i1.Mul(ristretto_magic);
            var rotate = T.Mul(z_inv).IsNegative();

            X.ConditionalAssign(iY, rotate);
            Y.ConditionalAssign(iX, rotate);
            den_inv.ConditionalAssign(enchanted_denominator, rotate);

            Y.ConditionalNegate(X.Mul(z_inv).IsNegative());

            var s = den_inv.Mul(Z.Sub(Y));
            var s_is_negative = s.IsNegative();
            s.ConditionalNegate(s_is_negative);

            return new CompressedRistretto(s.ToBytes());
        }
    }
}
