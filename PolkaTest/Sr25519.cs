namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Schnorrkel;
    using System.Text;
    using Schnorrkel.Merlin;
    using System.IO;
    using System;
    using Schnorrkel.Signed;

    public class Sr25519Test
    {
        private readonly ITestOutputHelper output;

        public Sr25519Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        public string PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public byte[] GetFileContent(string filename)
        {
            return File.ReadAllBytes(filename);
        }

        public class Hardcoded : RandomGenerator
        {
            private byte[] res;
            public Hardcoded(byte[] validResult)
            {
                res = validResult;
            }

            public override void FillBytes(ref byte[] dst)
            {
                dst = new byte[] { 77, 196, 92, 65, 167, 196, 215, 23, 222, 26, 136, 164, 123, 67, 115, 78, 178, 96, 208, 59, 8, 157, 203, 111, 157, 87, 69, 105, 155, 61, 111, 153 };
            }

            public byte[] Result()
            {
                return res;
            }
        }

        [Fact]
        public void Ok()
        {
            string prjPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            // set 1
            var sk1 = File.ReadAllBytes($"{prjPath}//TestData//secret1");
            var pk1 = File.ReadAllBytes($"{prjPath}//TestData//public1");
            var sn1 = File.ReadAllBytes($"{prjPath}//TestData//nonce1");
            var res1 = File.ReadAllBytes($"{prjPath}//TestData//result1");

            var rg1 = new Hardcoded(res1);

            var skBytes = new byte[64];
            sk1.CopyTo(skBytes, 0);
            sn1.CopyTo(skBytes, 32);

            var ctx = new SigningContext(Encoding.UTF8.GetBytes("good"));
            var pk = new PublicKey(pk1);
            var st = new SigningTranscript(ctx);
            var sk = SecretKey.FromBytes(skBytes);

            // send message
            var goodMessage = Encoding.UTF8.GetBytes("test message");
            ctx.ts = ctx.Bytes(goodMessage);

            var result = Sr25519.Sign(st, sk, pk, rg1);

            Assert.True(result.ToBytes().Equal(rg1.Result()));

            // set 2
            var sk2 = File.ReadAllBytes($"{prjPath}//TestData//secret2");
            var sn2 = File.ReadAllBytes($"{prjPath}//TestData//nonce2");
            var pk2 = File.ReadAllBytes($"{prjPath}//TestData//public2");
            var res2 = File.ReadAllBytes($"{prjPath}//TestData//result2");

            var rg2 = new Hardcoded(res2);

            var skBytes2 = new byte[64];
            sk2.CopyTo(skBytes2, 0);
            sn2.CopyTo(skBytes2, 32);

            var ctx2 = new SigningContext(Encoding.UTF8.GetBytes("good"));
            var pk22 = new PublicKey(pk2);
            var st22 = new SigningTranscript(ctx2);
            var sk22 = SecretKey.FromBytes(skBytes2);

            // send message
            var goodMessage2 = Encoding.UTF8.GetBytes("test message");
            ctx2.ts = ctx2.Bytes(goodMessage2);

            var result2 = Sr25519.Sign(st22, sk22, pk22, rg2);

            Assert.True(result2.ToBytes().Equal(rg2.Result()));

            //output.WriteLine(PrintByteArray(result.ToBytes()));
        }
    }
}
