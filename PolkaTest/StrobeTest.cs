namespace PolkaTest
{
    using Xunit;
    using Xunit.Abstractions;
    using Schnorrkel.Scalars;

    public class StrobeTest
    {
        private readonly ITestOutputHelper output;

        public StrobeTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            //var strobe = new Transcript(Encoding.UTF8.GetBytes("test"));

            //var witness = new byte[] { 131, 243, 135, 114, 255, 145, 65, 236, 163, 80, 229, 153, 254, 51, 89, 163, 103, 190, 248, 240, 171, 108, 210, 230, 150, 29, 241, 230, 73, 134, 12, 155 };
            //var _obj = new Strobe("Merlin v1.0", 128);
            //_obj.MKey(witness, false);


            var a = new FieldElement51(649930929663508,
                    982714405394398,
                    3329510941995740,
                    3600136455401671,
                    549649757566253);

            var b = new FieldElement51(199570966926459,
                    81994479920299,
                    1528071091047542,
                    2249056567190523,
                    99289794829204);

            var r  = a.Sub(b);

            var fff = 34534534;
            //var secretKey = new byte[] { 70, 179, 199, 139, 176, 96, 6, 129, 123, 203, 19, 80, 205, 143, 214, 116, 120, 207, 117, 35, 141, 244, 44, 235, 62, 191, 53, 186, 104, 220, 157, 10 };
            //var secretNonce = new byte[] { 131, 243, 135, 114, 255, 145, 65, 236, 163, 80, 229, 153, 254, 51, 89, 163, 103, 190, 248, 240, 171, 108, 210, 230, 150, 29, 241, 230, 73, 134, 12, 155 }; 
            //var publicKey = new byte[] { 8, 61, 91, 66, 212, 213, 204, 188, 29, 80, 242, 156, 87, 105, 55, 26, 128, 124, 21, 225, 240, 205, 197, 9, 165, 66, 131, 168, 81, 22, 148, 121 }; 

            ////var publicKeyRistretto = new RistrettoPoint(
            ////    new EdwardsPoint(
            ////        new FieldElement51(579512849318823, 342529492682387, 2001719311343173, 400031706137329, 1190376481251389),
            ////        new FieldElement51(1039675560930904, 1260362380430753, 184522769227591, 1553998222713081, 320159477204659),
            ////        new FieldElement51(1978696869913869, 1822725473332008, 825722946579470, 1229803275037522, 1709868382130165),
            ////        new FieldElement51(2162700505160831, 1308082099760856, 1269727569991316, 234972827165369, 443227757247997)
            ////));

            //var skBytes = new byte[64];
            //secretKey.CopyTo(skBytes, 0);
            //secretNonce.CopyTo(skBytes, 32);

            //var ctx = new SigningContext(Encoding.UTF8.GetBytes("good"));
            //var pk = new PublicKey(publicKey);
            //var st = new SigningTranscript(ctx, new SigningTranscriptOperation());
            //var sk = SecretKey.FromBytes(skBytes);

            //// send message
            //var goodMessage = Encoding.UTF8.GetBytes("test message");
            //ctx.Bytes(goodMessage);

            //var result = Sr25519.Sign(st, sk, pk);

            ////var data = Sr25519.Sign("0xd678b3e00c4238888bbf08dbbe1d7de77c3f1ca1fc71a5a283770f06f7cd1205",
            ////    "d678b3e00c4238888bbf08dbbe1d7de77c3f1ca1fc71a5a283770f06f7cd1205",
            ////    "empty message");



            //var resultGoodR = new byte[] { 44, 35, 234, 250, 150, 237, 162, 97, 64, 245, 85, 255, 226, 38, 121, 52, 185, 0, 79, 239, 191, 11, 14, 20, 205, 234, 88, 98, 108, 255, 83, 96 }; 
            //var resultGoodS = new byte[] { 177, 101, 187, 18, 15, 42, 235, 76, 132, 1, 1, 66, 189, 171, 216, 145, 144, 115, 187, 76, 255, 8, 76, 114, 127, 102, 29, 240, 31, 215, 202, 5 };

            //var resultBadR = new byte[] { 56, 83, 71, 77, 53, 117, 238, 249, 136, 220, 65, 4, 224, 129, 213, 95, 167, 246, 34, 183, 222, 249, 156, 20, 132, 50, 20, 34, 199, 14, 162, 102 }; 
            //var resultBadS = new byte[] { 136, 222, 150, 0, 173, 179, 195, 88, 18, 77, 207, 77, 211, 112, 37, 152, 231, 240, 185, 30, 81, 99, 244, 52, 139, 50, 131, 226, 90, 152, 50, 0 };



            //output.WriteLine(PrintByteArray(result.ToBytes()));
        }
    }
}
