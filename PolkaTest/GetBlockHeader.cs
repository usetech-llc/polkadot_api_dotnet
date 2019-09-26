namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class GetBlockHeader
    {
        private readonly ITestOutputHelper output;

        public GetBlockHeader(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            var testBlock = new GetBlockParams { BlockHash = "0x37096ff58d1831c2ee64b026f8b70afab1942119c022d1dcfdbdc1558ebf63fa" };

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                var result1 = app.GetBlock(testBlock);
                var result2 = app.GetBlockHeader(testBlock);

                output.WriteLine($"Parent hash from block  : { result1.Block.Header.ParentHash}");
                output.WriteLine($"Parent hash from header : { result2.ParentHash}");

                Assert.Equal(result1.Block.Header.ParentHash, result2.ParentHash);
                app.Disconnect();
            }
        }
    }
}
