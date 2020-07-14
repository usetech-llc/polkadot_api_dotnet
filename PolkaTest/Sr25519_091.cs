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
        public void Ok()
        {

            // Version 0.9.1
            // var message = new byte[] { 0 }; 
            // var message = Encoding.UTF8.GetBytes("aaa");

            // byte[] signerPublicKey = Converters.StringToByteArray("0xd678b3e00c4238888bbf08dbbe1d7de77c3f1ca1fc71a5a283770f06f7cd1205");
            // byte[] secretKeyVec = Converters.StringToByteArray("0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b");

            var message = Encoding.UTF8.GetBytes("aaa");


            byte[] pk = Converters.StringToByteArray("0x586cc32614d6a3f219667db501ade545753058d43b14e6e971e9c9cc908ad843");
            byte[] newSk = new byte[] { 25, 213, 29, 81, 62, 79, 15, 251, 133, 76, 195, 26, 105, 73, 195, 72, 250, 71, 29, 191, 218, 137, 226, 179, 177, 231, 181, 184, 231, 131, 87, 8, 34, 136, 220, 254, 5, 36, 13, 150, 131, 44, 182, 66, 174, 140, 83, 204, 30, 106, 8, 246, 45, 73, 25, 47, 15, 182, 26, 197, 26, 125, 25, 119 };
            var sig3 = Converters.StringToByteArray("0x30b9a6ee4c3f0ff13984cfe58c9673b832f14c70eedcc98f321825d99ad7191478bbbe692f2b4c2f5ddb3e43a05500ac523e73736c5577b6306b67257dfb5f80");

            // var sig2 = Sr25519v091.SignSimple(pk, newSk, message);



            var result = Sr25519v091.Verify(sig3, pk, message);

            Assert.True(Sr25519v091.Verify(sig3, pk, message));
        }
    }
}
