using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest.RpcTests
{
    public class OffchainRpcModuleTest
    {
        private readonly ITestOutputHelper _output;
        
        public OffchainRpcModuleTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Figure out how to use offchain storages.")]
        public async Task LocalStorageGet()
        {
            using var client = Constants.LocalClient(_output);
        }
    }
}