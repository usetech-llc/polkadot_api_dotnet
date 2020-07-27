using Polkadot.Api;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Xunit;

namespace PolkaTest
{
    public class AddressEncodingTest
    {
        [Fact]
        public void ToPublicToAddressDoesntChangeAddress()
        {
            var address = Constants.LocalAliceAddress;

            using var client = PolkaApi.GetApplication();
            client.Connect(Constants.LocalNodeUri);

            var key = AddressUtils.GetPublicKeyFromAddr(address);
            var decodedAddress = AddressUtils.GetAddrFromPublicKey(key);
            Assert.Equal(address.Symbols, decodedAddress);
        }
    }
}