using System.Threading.Tasks;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.State;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest.RpcTests
{
    public class StateModuleTest
    {
        private readonly ITestOutputHelper _output;

        public StateModuleTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetMetadata()
        {
            using var client = Constants.LocalClient(_output);
            var metadata = await client.Rpc.State().GetMetadata<Hash256>();
            Assert.Equal(12, metadata.RuntimeMetadata.Index);
        }
    }
}