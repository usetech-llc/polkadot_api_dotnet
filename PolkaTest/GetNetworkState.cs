using Polkadot.Exceptions;

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

        [Fact(Skip = "Unsafe methods are not allowed.")]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect(Constants.LocalNodeUri);
                var result = app.GetNetworkState();

                Assert.NotNull(result);

                output.WriteLine($"AverageDownloadPerSec : {result.AverageDownloadPerSec}");
                output.WriteLine($"AverageUploadPerSec : {result.AverageUploadPerSec}");
                output.WriteLine($"PeerId : {result.PeerId}");
                output.WriteLine($"Peerset : {result.Peerset}");

                app.Disconnect();
            }
        }

        [Fact]
        public void UnsafeCallThrowsCorrectException()
        {
            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect();
                var exception = Assert.Throws<JrpcErrorException>(() => app.GetNetworkState());
                app.Disconnect();
            }
        }
    }
}
