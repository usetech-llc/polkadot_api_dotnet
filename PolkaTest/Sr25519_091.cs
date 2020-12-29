namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Text;
    using System.IO;
    using System;
    using Schnorrkel;
    using Polkadot.Utils;
    using Schnorrkel.Keys;
    using StrobeNet.Extensions;

    public class Sr25519_091Test
    {
        private readonly ITestOutputHelper output;

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public Sr25519_091Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SignWithMiniSecret()
        {
            //    Secret Key URI `//Alice` is account:
            //    Network ID/ version: substrate
            //    Secret seed:        0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
            //    Public key(hex):    0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
            //    Account ID:         0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
            //    SS58 Address:       5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY

            // Version 0.9.1
            var message = Encoding.UTF8.GetBytes("aaa");

            var msk = new MiniSecret("e5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a".ToByteArray(), 
                ExpandMode.Ed25519);

            // Check public key generation
            var pair = msk.GetPair();
            Assert.Equal("d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d",
                pair.Public.Key.ToHexString().ToLower());

            var sig = Sr25519v091.SignSimple(pair, message);
            var verified = Sr25519v091.Verify(sig, pair.Public, message);

            Assert.True(verified);
        }

        [Fact]
        public void SignWithRawKeys()
        {

            // Version 0.9.1
            var message = Encoding.UTF8.GetBytes("aaa");

            byte[] rawPublic = "d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d".HexToByteArray();
            byte[] rawSecret = "33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011".HexToByteArray();

            var sig = Sr25519v091.SignSimple(rawPublic, rawSecret, message);
            var verified = Sr25519v091.Verify(sig, rawPublic, message);

            Assert.True(verified);
        }
    }
}
