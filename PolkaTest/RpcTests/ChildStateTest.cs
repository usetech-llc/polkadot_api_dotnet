using System.Threading.Tasks;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.Сhildstate;
using Polkadot.BinarySerializer.Types;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest.RpcTests
{
    public class ChildStateTest
    {
        private readonly ITestOutputHelper _output;

        public ChildStateTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Find some always exising keys to test on.")]
        public async Task GetKeys()
        {
            using var client = Constants.LocalClient(_output);

            var keys = await client.Rpc.ChildState().GetKeys<StorageKey, PrefixedStorageKey, StorageKey>(new byte[] {1, 2, 3}, new byte[] {1, 2, 3});
        }

        [Fact(Skip = "Find some always exising keys to test on.")]
        public async Task GetStorage()
        {
            using var client = Constants.LocalClient(_output);

            var state = await client.Rpc.ChildState().GetStorage<StorageKey, PrefixedStorageKey, Unit>(new byte[] {1, 2, 3}, new byte[] {1, 2, 3});
        }

        [Fact(Skip = "Find some always exising keys to test on.")]
        public async Task GetStorageHash()
        {
            using var client = Constants.LocalClient(_output);

            var hash = await client.Rpc.ChildState().GetStorageHash<StorageKey, PrefixedStorageKey>(new byte[] {1, 2, 3}, new byte[] {1, 2, 3});
        }

        [Fact(Skip = "Find some always exising keys to test on.")]
        public async Task GetStorageSize()
        {
            using var client = Constants.LocalClient(_output);

            var size = await client.Rpc.ChildState().GetStorageSize<StorageKey, PrefixedStorageKey>(new byte[] {1, 2, 3}, new byte[] {1, 2, 3});
        }
    }
}