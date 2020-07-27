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
            var testBlock = new GetBlockParams { BlockHash = "0xbd6c40490d8272f2c03bb19627add2ac6fddc1fd7e0eefd690ca47e4f9729d8f" };

            using (IApplication app = PolkaApi.GetApplication())
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
