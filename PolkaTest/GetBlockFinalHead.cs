namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class GetBlockFinalHead
    {
        private readonly ITestOutputHelper output;

        public GetBlockFinalHead(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                var result = app.GetFinalizedHead();

                output.WriteLine($"Finalized head hash: { result.BlockHash }");

                Assert.True(result.BlockHash.Length == 66);
                app.Disconnect();
            }
        }
    }
}
