using System.Threading.Tasks;
using Polkadot.Api.Client.Modules.State;
using Xunit;

namespace PolkaTest.RpcTest
{
    public class StateModuleTest
    {
        [Fact]
        public async Task GetMetadata()
        {
            using var client = Constants.LocalClient();
            var metadata = await client.Rpc.State().GetMetadata();
            Assert.Equal(12, metadata.RuntimeMetadata.Index);
        }
    }
}