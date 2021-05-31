using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest.RpcTests
{
    public class PaymentModuleTest
    {
        private readonly ITestOutputHelper _output;
        
        public PaymentModuleTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Figure out how to test it")]
        public async Task QueryInfo()
        {
            using var client = Constants.LocalClient(_output);
        }
    }
}