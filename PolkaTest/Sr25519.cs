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

    public class Sr25519Test
    {
        private readonly ITestOutputHelper output;

        public Sr25519Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            const long payloadSize = 20;

            // Version 0.1.1
            var signaturePayloadBytes = new byte[payloadSize];
            var rnd = new Random();
            rnd.NextBytes(signaturePayloadBytes);
            long payloadLength = payloadSize;

            byte[] signerPublicKey = Converters.HexToByteArray("0xd678b3e00c4238888bbf08dbbe1d7de77c3f1ca1fc71a5a283770f06f7cd1205");
            byte[] secretKeyVec = Converters.HexToByteArray("0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b");

            var message = signaturePayloadBytes.AsMemory().Slice(0, (int)payloadLength).ToArray();
            var sig2 = Sr25519v011.SignSimple(signerPublicKey, secretKeyVec, message);

            Console.Write("Message: 0x");
            for (var i=0; i<payloadSize; i++) {
                Console.Write($"{signaturePayloadBytes[i]:X2}");
            }
            Console.WriteLine("");

            Console.Write("Signature: 0x");
            for (var i=0; i<sig2.Length; i++) {
                Console.Write($"{sig2[i]:X2}");
            }
            Console.WriteLine("");


            Assert.True(Sr25519v011.Verify(sig2, signerPublicKey, message));
        }
    }
}
