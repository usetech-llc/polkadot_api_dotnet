using System.Threading;
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
            var client = SubstrateClient.FromSettings(SubstrateClientSettings.Default() with {RpcEndpoint = "ws://10.10.10.10/"});
            var exception = await Assert.ThrowsAsync<TransportException>(() => client.Rpc.Call<string>("rpc does not exist", CancellationToken.None));
            Assert.True(exception.IsConnectionTimeout());
        }
    }
}