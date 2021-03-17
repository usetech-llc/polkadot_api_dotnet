using System.Numerics;
using Mnemonic;
using Polkadot.Api;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Xunit;

namespace PolkaTest
{
    public class AccountInfoTest
    {
        [Fact]
        public void NewAccountNonceIsZero()
        {
            using IApplication app = PolkaApi.GetApplication();
            app.Connect(Constants.LocalNodeUri);
            var key = MnemonicSubstrate
                .GeneratePairFromMnemonic(
                    "twice unfold problem output detail jewel item engage visit carry eight green")
                .Public.Key;
            var address = new Address(AddressUtils.GetAddrFromPublicKey(new PublicKey() {Bytes = key}));
            var nonce = app.GetAccountNonce(address);
            Assert.Equal(BigInteger.Zero, nonce);
        }
    }
}