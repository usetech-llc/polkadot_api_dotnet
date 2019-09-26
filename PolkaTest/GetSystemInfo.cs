namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;

    [Collection("Sequential")]
    public class GetSystemInfo
    {
        private readonly ITestOutputHelper output;

        public GetSystemInfo(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                var result = app.GetSystemInfo();

                Assert.True(result.ChainId.Length > 0);

                // Check chainName
                Assert.Equal("parity-polkadot", result.ChainName);

                // Check version
                Assert.NotEqual(string.Empty, result.Version);

                // Check tokenSymbol
                Assert.Equal("DOT", result.TokenSymbol);

                // Check tokenDecimals
                Assert.Equal(15, result.TokenDecimals);

                output.WriteLine($"Chain id        : {result.ChainId}");
                output.WriteLine($"Chain name      : {result.ChainName}");
                output.WriteLine($"Version         : {result.Version}");
                output.WriteLine($"Token symbol    : {result.TokenSymbol}");
                output.WriteLine($"Token decimals  : {result.TokenDecimals}");

                app.Disconnect();
            }
        }
    }
}
