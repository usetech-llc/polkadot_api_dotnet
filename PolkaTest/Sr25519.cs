namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Text;
    using System.IO;
    using System;
    //using Schnorrkel.Signed;
    //using Schnorrkel.Merlin;
    //using Schnorrkel;

    public class Sr25519Test
    {
        private readonly ITestOutputHelper output;

        public Sr25519Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        //public string PrintByteArray(byte[] bytes)
        //{
        //    var sb = new StringBuilder("new byte[] { ");
        //    foreach (var b in bytes)
        //    {
        //        sb.Append(b + ", ");
        //    }
        //    sb.Append("}");
        //    return sb.ToString();
        //}

        //public byte[] GetFileContent(string filename)
        //{
        //    return File.ReadAllBytes(filename);
        //}

        //public class Hardcoded : RandomGenerator
        //{
        //    private byte[] _res;
        //    public Hardcoded(byte[] validResult)
        //    {
        //        _res = validResult;
        //    }

        //    public override void FillBytes(ref byte[] dst)
        //    {
        //        dst = new byte[] { 77, 196, 92, 65, 167, 196, 215, 23, 222, 26, 136, 164, 123, 67, 115, 78, 178, 96, 208, 59, 8, 157, 203, 111, 157, 87, 69, 105, 155, 61, 111, 153 };
        //    }

        //    public byte[] Result()
        //    {
        //        return _res;
        //    }
        //}

        [Fact]
        public void Ok()
        {
            //string prjPath2 = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            //var sk11 = File.ReadAllBytes($"J://projects//schnorrkel//testSig");
            //var sdfdsf = sk11.PrintByteArray(); 
            //////
            //string prjPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            //// set 1
            //var sk1 = File.ReadAllBytes($"{prjPath}//TestData//secret1");
            //var pk1 = File.ReadAllBytes($"{prjPath}//TestData//public1");
            //var sn1 = File.ReadAllBytes($"{prjPath}//TestData//nonce1");
            //var res1 = File.ReadAllBytes($"{prjPath}//TestData//result1");

            //var rg1 = new Hardcoded(res1);

            //var sr25519TestCtx = new Sr25519(new SchnorrkelSettings
            //{
            //    RandomGenerator = rg1,
            //    SigningContextMessage = "good"
            //});

            //var skBytes = new byte[64];
            //sk1.CopyTo(skBytes, 0);
            //sn1.CopyTo(skBytes, 32);

            //var result = sr25519TestCtx.Sign(skBytes, pk1, Encoding.UTF8.GetBytes("test message"));
            //Assert.True(result.ToBytes().Equal(rg1.Result()));

            //// set 2
            //var sk2 = File.ReadAllBytes($"{prjPath}//TestData//secret2");
            //var sn2 = File.ReadAllBytes($"{prjPath}//TestData//nonce2");
            //var pk2 = File.ReadAllBytes($"{prjPath}//TestData//public2");
            //var res2 = File.ReadAllBytes($"{prjPath}//TestData//result2");

            //var rg2 = new Hardcoded(res2);

            //var sr25519TestCtx2 = new Sr25519(new SchnorrkelSettings
            //{
            //    RandomGenerator = rg1,
            //    SigningContextMessage = "good"
            //});

            //var skBytes2 = new byte[64];
            //sk2.CopyTo(skBytes2, 0);
            //sn2.CopyTo(skBytes2, 32);

            //var result2 = sr25519TestCtx2.Sign(skBytes2, pk2, Encoding.UTF8.GetBytes("test message"));
            //Assert.True(result2.ToBytes().Equal(rg2.Result()));
        }
    }
}
