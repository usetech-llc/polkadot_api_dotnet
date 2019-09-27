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
                app.Connect();
                var result = app.GetSystemPeers();

                Assert.True(result != null);

                output.WriteLine($"Count : {result.Count}");
                output.WriteLine($"Peers : {result.Peers}");

                app.Disconnect();
            }
        }
    }
}
