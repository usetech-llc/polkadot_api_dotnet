namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class GetBlockHash
    {
        private readonly ITestOutputHelper output;

        public GetBlockHash(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                var result = app.GetBlockHash(new GetBlockHashParams { BlockNumber = 2 });

                Assert.Equal("0x2df84d4c6bb8441f7a1702b4589dc33f8dc43a0794a4df446c74110636456989", result.Hash);

                output.WriteLine($"Result hash : {result.Hash}");

                app.Disconnect();
            }
        }
    }
}
