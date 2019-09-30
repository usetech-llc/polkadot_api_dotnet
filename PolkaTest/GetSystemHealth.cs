namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class GetSystemHealth
    {
        private readonly ITestOutputHelper output;

        public GetSystemHealth(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                var result = app.GetSystemHealth();

                Assert.True(result != null);

                output.WriteLine($"Peers : {result.Peers}");
                output.WriteLine($"ShouldHavePeers : {result.ShouldHavePeers}");

                app.Disconnect();
            }
        }
    }
}
