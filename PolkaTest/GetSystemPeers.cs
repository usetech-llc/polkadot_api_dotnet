using Polkadot.Exceptions;

namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class GetSystemPeers
    {
        private readonly ITestOutputHelper output;

        public GetSystemPeers(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect(Constants.LocalNodeUri);
                var result = app.GetSystemPeers();

                Assert.NotNull(result);

                output.WriteLine($"Count : {result.Count}");
                output.WriteLine($"Peers : {result.Peers}");

                app.Disconnect();
            }
        }

        [Fact]
        public void UnsafeCallThrowsCorrectException()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                var exception = Assert.Throws<UnsafeNotAllowedException>(() => app.GetSystemPeers());
                Assert.Contains("system_peers", exception.Message);

                app.Disconnect();
            }
        }
    }
}
