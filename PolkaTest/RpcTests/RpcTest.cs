using System;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client;
using Polkadot.Api.Client.Modules.Chain;
using Polkadot.Api.Client.Modules.State.Model;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.Utils;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest.RpcTests
{
    public class RpcTest
    {
        private readonly ITestOutputHelper _output;

        public RpcTest(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public async Task NotExistingRpcThrowsError()
        {
            using var client = Constants.LocalClient();
            await Assert.ThrowsAsync<JrpcErrorException<TextJsonElement>>(() => client.Rpc.Call<string>("this rpc should not exist", CancellationToken.None));
        }

        [Fact]
        public async Task ShortTimeoutThrowsTimeoutException()
        {
            using var client = SubstrateClient.FromSettings(SubstrateClientSettings.Default() with
            {
                RpcEndpoint = Constants.LocalNodeUri,
                RpcTimeout = TimeSpan.FromSeconds(0)
            });
            await Assert.ThrowsAsync<TimeoutException>(() =>
                client.Rpc.Call<RuntimeMetadataPrefixed>("state_getMetadata", CancellationToken.None));
        }

        [Fact]
        public async Task SubscribeWorks()
        {
            using var client = Constants.LocalClient(_output);
            
            var taskCompletionSource = new TaskCompletionSource();
            await client.Rpc.Chain().SubscribeNewHead(m =>
            {
                m.Switch(a => taskCompletionSource.SetResult(), ex => taskCompletionSource.SetException(ex));
                return Task.CompletedTask;
            });

            await taskCompletionSource.Task.WithTimeout(TimeSpan.FromMinutes(1));
        }
    }
}