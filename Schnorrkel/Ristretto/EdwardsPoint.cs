namespace Schnorrkel.Ristretto
{
    using Schnorrkel.Scalars;

    public class EdwardsPoint
    {
        public FieldElement51 X, Y, Z, T;

        public EdwardsPoint() {}

        public EdwardsPoint(FieldElement51 x, FieldElement51 y, FieldElement51 z, FieldElement51 t)
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }

        public bool Equals(EdwardsPoint a)
        {
            bool result = true;
            for (var i = 0; i < 5; i++)
            {
                result &= X._data[i] == a.X._data[i]
                    && Y._data[i] == a.Y._data[i]
                    && Z._data[i] == a.Z._data[i]
                    && T._data[i] == a.T._data[i];
            }

            return result;
        }

        public EdwardsPoint Copy()
        {
            return new EdwardsPoint
            {
                T = T,
                X = X,
                Y = Y,
                Z = Z
            };
        }

        /// Compute \\([2\^k] P \\) by successive doublings. Requires \\( k > 0 \\).
        public EdwardsPoint MulByPow2(int k)
        {
            CompletedPoint r;
            var s = ToProjective();
            for (var i = 0; i < k - 1; i++)
            {
                r = s.Double();
                s = r.ToProjective();
            }

            // Unroll last iteration so we can go directly to_extended()
            return s.Double().ToExtended();
        }

        public static FieldElement51 Zero()
        {
            return new FieldElement51 { _data = new ulong[] { 0, 0, 0, 0, 0 } };
        }

        public static FieldElement51 One()
        {
            return new FieldElement51 { _data = new ulong[] { 1, 0, 0, 0, 0 } };
        }

        public static EdwardsPoint Identity()
        {
            return new EdwardsPoint
            {
                X = Zero(),
                Y = One(),
                Z = One(),
                T = Zero()
            };
        }

        EdwardsPoint ToExtended()
        {
            return new EdwardsPoint
            {
                X = X.Mul(T),
                Y = Y.Mul(Z),
                Z = Z.Mul(T),
                T = X.Mul(Y)
            };
        }

        public CompletedPoint Add(ProjectiveNielsPoint other)
        {
            var Y_plus_X = Y.Add(X);
            var Y_minus_X = Y.Sub(X);
            var PP = Y_plus_X.Mul(other.Y_plus_X);
            var MM = Y_minus_X.Mul(other.Y_minus_X);
            var TT2d = T.Mul(other.T2d);
            var ZZ = Z.Mul(other.Z);
            var ZZ2 = ZZ.Add(ZZ);

            return new CompletedPoint
            {
                X = PP.Sub(MM),
                Y = PP.Add(MM),
                Z = ZZ2.Add(TT2d),
                T = ZZ2.Sub(TT2d)
            };
        }

        public CompletedPoint Sub(ProjectiveNielsPoint other)
        {
            var Y_plus_X = Y.Add(X);
            var Y_minus_X = Y.Sub(X);
            var PM = Y_plus_X.Mul(other.Y_minus_X);
            var MP = Y_minus_X.Mul(other.Y_plus_X);
            var TT2d = T.Mul(other.T2d);
            var ZZ = Z.Mul(other.Z);
            var ZZ2 = ZZ.Add(ZZ);

            return new CompletedPoint
            {
                X = PM.Sub(MP),
                Y = PM.Add(MP),
                Z = ZZ2.Sub(TT2d),
                T = ZZ2.Add(TT2d)
            };
        }

        public CompletedPoint Add(AffineNielsPoint other)
        {
            var Y_plus_X = Y.Add(X);
            var Y_minus_X = Y.Sub(X);
            var PP = Y_plus_X.Mul(other.Y_plus_X);
            var MM = Y_minus_X.Mul(other.Y_minus_X);
            var Txy2d = T.Mul(other.XY2d);
            var Z2 = Z.Add(Z);

            return new CompletedPoint
            {
                X = PP.Sub(MM),
                Y = PP.Add(MM),
                Z = Z2.Add(Txy2d),
                T = Z2.Sub(Txy2d)
            };
        }

        public EdwardsPoint Add(EdwardsPoint other)
        {
            return Add(other.ToProjectiveNiels()).ToExtended();
        }

        public ProjectiveNielsPoint ToProjectiveNiels()
        {
            return new ProjectiveNielsPoint
            {
                Y_plus_X = Y.Add(X),
                Y_minus_X = Y.Sub(X),
                Z = Z,
                T2d = T.Mul(Consts.EDWARDS_D2)
            };
        }

        public ProjectivePoint ToProjective()
        {
            return new ProjectivePoint
            {
                X = X,
                Y = Y,
                Z = Z
            };
        }

        public AffineNielsPoint ToAffineNiels()
        {
            var recip = Z.Invert();
            var x = X.Mul(recip);
            var y = Y.Mul(recip);
            var xy2d = X.Mul(Y).Mul(Consts.EDWARDS_D2);
            return new AffineNielsPoint
            {
                Y_plus_X = y.Add(x),
                Y_minus_X = y.Sub(x),
                XY2d = xy2d
            };
        }
    }
}
