using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.Chain;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Types;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest.RpcTests
{
    public class ChainModuleTest
    {
        private readonly ITestOutputHelper _output;

        public ChainModuleTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetBlock()
        {
            using var client = Constants.LocalClient(_output);

            var block = await client.Rpc.Chain().GetBlock(null);
            
            Assert.Equal(32, block.Block.Header.ExtrinsicsRoot.Value.Length);
            Assert.Equal(32, block.Block.Header.ParentHash.Value.Length);
            Assert.Equal(32, block.Block.Header.StateRoot.Value.Length);
        }

        [Fact]
        public async Task GetBlockHashNoParameters()
        {
            using var client = Constants.LocalClient(_output);

            var hash = await client.Rpc.Chain().GetBlockHash();
            Assert.Equal(32, hash.Value.Length);
        }

        [Fact]
        public async Task GetBlockHashOneParameter()
        {
            using var client = Constants.LocalClient(_output);

            var hash = await client.Rpc.Chain().GetBlockHash(1);
            Assert.Equal(32, hash.Value.Length);
        }

        [Fact]
        public async Task GetBlockHashListOfParams()
        {
            using var client = Constants.LocalClient(_output);

            var hashes = await client.Rpc.Chain().GetBlockHash(new long[] {1, 2, 3});
            Assert.Equal(3, hashes.Length);
            Assert.All(hashes, h => Assert.Equal(32, h.Value.Length));
        }

        [Fact]
        public async Task GetFinalizedHead()
        {
            using var client = Constants.LocalClient(_output);

            var hash = await client.Rpc.Chain().GetFinalizedHead();
            Assert.Equal(32, hash.Value.Length);
        }

        [Fact]
        public async Task GetHeader()
        {
            using var client = Constants.LocalClient(_output);

            var header = await client.Rpc.Chain().GetHeader();
            
            Assert.True(header.Number > 0);
            Assert.Equal(32, header.ExtrinsicsRoot.Value.Length);
            Assert.Equal(32, header.ParentHash.Value.Length);
            Assert.Equal(32, header.StateRoot.Value.Length);

            var firstBlockHash = await client.Rpc.Chain().GetBlockHash(1);
            var firstBlockHeader = await client.Rpc.Chain().GetHeader(firstBlockHash);
            Assert.Equal(1, firstBlockHeader.Number);
        }

        [Fact]
        public async Task SubscribeAllHeads()
        {
            using var client = Constants.LocalClient(_output);

            var tcs = new TaskCompletionSource<Header<long, Hash256>>();

            var subscription = await client.Rpc.Chain()
                .SubscribeAllHeads(t =>
                {
                    t.Switch(
                        h => tcs.SetResult(h),
                        e => tcs.SetException(e));
                    return Task.CompletedTask;
                });
            await tcs.Task;
            
            Assert.True(tcs.Task.Result.Number > 0);
            await subscription.Unsubscribe();
        }

        [Fact]
        public async Task SubscribeNewHead()
        {
            using var client = Constants.LocalClient(_output);

            var tcs = new TaskCompletionSource<Header<long, Hash256>>();

            var subscription = await client.Rpc.Chain()
                .SubscribeNewHead(t =>
                {
                    t.Switch(
                        h => tcs.SetResult(h),
                        e => tcs.SetException(e));
                    return Task.CompletedTask;
                });
            await tcs.Task;
            
            Assert.True(tcs.Task.Result.Number > 0);
            await subscription.Unsubscribe();
        }
        
        [Fact]
        public async Task SubscribeFinalizedHeads()
        {
            using var client = Constants.LocalClient(_output);

            var tcs = new TaskCompletionSource<Header<long, Hash256>>();

            var subscription = await client.Rpc.Chain()
                .SubscribeFinalizedHeads(t =>
                {
                    t.Switch(
                        h => tcs.SetResult(h),
                        e => tcs.SetException(e));
                    return Task.CompletedTask;
                });
            await tcs.Task;
            
            Assert.True(tcs.Task.Result.Number > 0);
            await subscription.Unsubscribe();
        }
    }
}