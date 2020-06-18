namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class GetNetworkState
    {
        private readonly ITestOutputHelper output;

        public GetNetworkState(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect("wss://kusama-rpc.polkadot.io/");
                var result = app.GetNetworkState();

                Assert.True(result != null);

                output.WriteLine($"AverageDownloadPerSec : {result.AverageDownloadPerSec}");
                output.WriteLine($"AverageUploadPerSec : {result.AverageUploadPerSec}");
                output.WriteLine($"PeerId : {result.PeerId}");
                output.WriteLine($"Peerset : {result.Peerset}");

                app.Disconnect();
            }
        }
    }
}
