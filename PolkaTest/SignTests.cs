using System;
using System.Linq;
using System.Text;
using Polkadot.Api;
using Polkadot.BinaryContracts;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Schnorrkel;
using Xunit;

namespace PolkaTest
{
    public class SignTests
    {
        [Fact]
        public void SignedMessageVerifies()
        {
            var message = new byte[] {0};
            var random = new Random();
            var message2 = new byte[1000];
            random.NextBytes(message2);

            using var application = (Application)PolkaApi.GetApplication();
            application.Connect(Constants.LocalNodeUri);
            var publicKey = application._protocolParams.Metadata.GetPublicKeyFromAddr(Constants.LocalAliceAddress).Bytes;
            var privateKey = Constants.LocalAlicePrivateKey;
            
            var sign = (ExtrinsicMultiSignature)application.Signer.Sign(publicKey, privateKey, message);
            var verify = application.Signer.VerifySignature(sign.Signature, publicKey, message);
            Assert.True(verify);

            var sign2 = (ExtrinsicMultiSignature)application.Signer.Sign(publicKey, privateKey, message2);
            var verify2 = application.Signer.VerifySignature(sign2.Signature, publicKey, message2);
            Assert.True(verify2);
        }
        
        [Fact]
        public void VerificationOfWebSignSuccessful()
        {
            using var application = (Application)PolkaApi.GetApplication();
            application.Connect(Constants.LocalNodeUri);

            var message = new byte[]{0};
            var webSign = "0xb0633663163e6149e1f1a429dbd606284d28bdb517c107d2384c2aadb6b95671089e97f2ccbbe2e4f15760b03b84e70e57845a61234b7382bdde0d8f653a908b";

            var hexToByteArray = webSign.HexToByteArray();
            var alicePublicKey = application._protocolParams.Metadata.GetPublicKeyFromAddr(Constants.LocalAliceAddress).Bytes;
            var verify = application.Signer.VerifySignature(
                hexToByteArray,
                alicePublicKey,
                message);
            
            Assert.True(verify);
        }
    }
}