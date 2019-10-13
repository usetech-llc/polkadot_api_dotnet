namespace Schnorrkel
{
    using Schnorrkel.Merlin;
    using Schnorrkel.Ristretto;
    using Schnorrkel.Scalars;
    using Schnorrkel.Signed;
    using System.Text;

    public class Sr25519
    {
        private RandomGenerator _rng;

        public Sr25519(SchnorrkelSettings settings)
        {
            _rng = settings.RandomGenerator;
        }

        public Signature Sign(SigningTranscript st, SecretKey secretKey, PublicKey publicKey)
        {
            return Sign(st, secretKey, publicKey, _rng);
        }

        public static byte[] SignSimple(string publicKey, string secretKey, string message)
        {
            var sk = SecretKey.FromBytes(Encoding.UTF8.GetBytes(secretKey));
            var signingContext = new SigningContext(Encoding.UTF8.GetBytes(message));
            var pk = new PublicKey(Encoding.UTF8.GetBytes(publicKey));
            var st = new SigningTranscript(signingContext);

            var sig = Sign(st, sk, pk, null);
            
            return sig.ToBytes();
        }

        private static byte[] GetStrBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static Signature Sign(SigningTranscript st, SecretKey secretKey, PublicKey publicKey, RandomGenerator rng)
        {
            st.SetProtocolName(GetStrBytes("Schnorr-sig"));
            st.CommitPoint(GetStrBytes("sign:pk"), publicKey.Key);

            var r = st.WitnessScalar(GetStrBytes("signing"), secretKey.nonce, rng);

            var tbl = new RistrettoBasepointTable();
            var R = tbl.Mul(r).Compress();

            st.CommitPoint(GetStrBytes("sign:R"), R);

            Scalar k = st.ChallengeScalar(GetStrBytes("sign:c"));  // context, message, A/public_key, R=rG
            k.Recalc();
            secretKey.key.Recalc();
            r.Recalc();

            var t1 = Scalar52.Mul(k.ScalarInner, secretKey.key.ScalarInner);
            var t2 = Scalar52.Add(t1, r.ScalarInner);

            var scalarBytes = Scalar52.Add(Scalar52.Mul(k.ScalarInner, secretKey.key.ScalarInner), r.ScalarInner).ToBytes();

            var s = new Scalar { ScalarBytes = scalarBytes };
            s.Recalc();

            return new Signature { R = R, S = s };
        }
    }
}
