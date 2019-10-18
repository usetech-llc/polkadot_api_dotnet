namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using System.IO;
    using sr25519_dotnet.lib.Models;
    using sr25519_dotnet.lib;
    using System;
    using System.Numerics;
    using System.Linq;
    using System.Text;
    using Polkadot.Utils;

    [Collection("Sequential")]
    public class SignLibTest
    {
        private readonly ITestOutputHelper output;

        public SignLibTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            BigInteger seed = new BigInteger(DateTime.Now.Ticks) << DateTime.Now.Second;

            var rnd = new Random(BitConverter.ToInt32(seed.ToByteArray().Take(4).ToArray()));
            var publicKey = new byte[32];
            var secretKey = new byte[64];
            var payload = new byte[64];

            rnd.NextBytes(publicKey);
            rnd.NextBytes(secretKey);
            rnd.NextBytes(payload);

            var pk = AddressUtils.GetPublicKeyFromAddr("5GWYBLjRtCQLXQmcyyRa6KaF1ihuqLjvVDE2gswJsEMxd9Qm");

            var kp = new SR25519Keypair(pk.Bytes, secretKey);
            var sig = SR25519.Sign(payload, (ulong)payload.Length, kp);

            var arrayNotSame = new Func<byte[], bool>((btArr) => {
                return btArr.GroupBy((i) => i).Count() > 50;
            });

            Assert.True(sig.Length == 64);
            Assert.True(arrayNotSame(sig));
        }
    }
}
