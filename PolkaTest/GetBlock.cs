namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;

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

                Assert.Equal("0x37096ff58d1831c2ee64b026f8b70afab1942119c022d1dcfdbdc1558ebf63fa", result.Hash);

                output.WriteLine($"Result hash : {result.Hash}");

                app.Disconnect();
            }
        }
    }
}
