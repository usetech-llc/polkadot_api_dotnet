using System.Threading.Tasks;
using Polkadot.Api.Client.Modules.Author;
using Polkadot.Utils;
using Xunit;

namespace PolkaTest.RpcTests
{
    public class AuthorTest
    {
        [Fact(Skip = "Method not found")]
        public async Task AliceHasKey()
        {
            using var client = Constants.LocalClient();
            var hasKey = await client.Rpc.Author()
                .HasKey(AddressUtils.GetPublicKeyFromAddr(Constants.LocalAliceAddress).Bytes, "Sr25519");
            
            Assert.True(hasKey);
        }

        [Fact]
        public async Task PendingExtrinsics()
        {
            using var client = Constants.LocalClient();
            var extrinsics = await client.Rpc.Author().PendingExtrinsics();
        }

        [Fact(Skip = "Method not found")]
        public async Task RotateKeys()
        {
            using var client = Constants.LocalClient();
            var rotate = await client.Rpc.Author().RotateKeys();
        }
    }
}