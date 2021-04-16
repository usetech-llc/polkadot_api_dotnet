using System;
using System.Threading.Tasks;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.DataStructs.Metadata.BinaryContracts;
using Xunit;

namespace PolkaTest.RpcTest
{
    public class RpcTest
    {
        [Fact]
        public async Task NotExistingRpcThrowsError()
        {
            using var client = Constants.LocalClient();
            await Assert.ThrowsAsync<JrpcErrorException>(() => client.Rpc.Call<string>("this rpc should not exist"));
        }

        [Fact]
        public async Task ShortTimeoutThrowsTimeoutException()
        {
            using var client = Constants.LocalClient();
            await Assert.ThrowsAsync<TimeoutException>(() =>
                client.Rpc.Call<RuntimeMetadataPrefixed>("state_getMetadata", timeout: TimeSpan.Zero));
        }
    }
}