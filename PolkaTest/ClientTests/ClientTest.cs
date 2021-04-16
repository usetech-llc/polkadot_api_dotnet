using System.Threading.Tasks;
using Polkadot.Api.Client;
using Polkadot.Api.Client.Exceptions;
using Xunit;

namespace PolkaTest.ClientTests
{
    public class ClientTest
    {
        [Fact]
        public async Task ConnectingToUnexistingUrlThrowsTransportException()
        {
            var client = new SubstrateClient(SubstrateClientSettings.Default() with {RpcEndpoint = "ws://0.0.0.0/"});
            await Assert.ThrowsAsync<TransportException>(() => client.Rpc.Call<string>("rpc does not exist"));
        }
    }
}