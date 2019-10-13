namespace Schnorrkel.Ristretto
{
    using Schnorrkel.Scalars;

    public class ProjectivePoint
    {
        public FieldElement51 X { get; set; }
        public FieldElement51 Y { get; set; }
        public FieldElement51 Z { get; set; }

        public CompletedPoint Double()
        {
            var XX = X.Square();
            var YY = Y.Square();
            var ZZ2 = Z.Square2();
            var X_plus_Y = X.Add(Y);
            var X_plus_Y_sq = X_plus_Y.Square();
            var YY_plus_XX = YY.Add(XX);
            var YY_minus_XX = YY.Sub(XX);

            return new CompletedPoint
            {
                X = X_plus_Y_sq.Sub(YY_plus_XX),
                Y = YY_plus_XX,
                Z = YY_minus_XX,
                T = ZZ2.Sub(YY_minus_XX)
            };
        }
    }
}
