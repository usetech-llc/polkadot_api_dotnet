using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest.RpcTests
{
    public class ContractsModuleTest
    {
        private readonly ITestOutputHelper _output;

        public ContractsModuleTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Need upload contract extrinsic to test that.")]
        public async Task Call()
        {
            using var client = Constants.LocalClient(_output);
        }

        [Fact(Skip = "Need upload contract extrinsic to test that.")]
        public async Task GetStorage()
        {
            using var client = Constants.LocalClient(_output);
        }

        [Fact(Skip = "Need upload contract extrinsic to test that.")]
        public async Task RentProjection()
        {
            using var client = Constants.LocalClient(_output);
        }
    }
}