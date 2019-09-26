namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Threading;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class WssubscribeAll
    {
        private readonly ITestOutputHelper output;

        public WssubscribeAll(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                long blockNum = 0;
                int messagesCount = 0;
                RuntimeVersion rv = null;

                app.Connect();

                int subId = app.SubscribeBlockNumber((blockNumber) =>
                {
                    blockNum = blockNumber;
                    messagesCount++;
                    output.WriteLine($"Last block number        : {blockNumber}");
                });

                int subId2 = app.SubscribeRuntimeVersion((runtimeVersion) =>
                {
                    rv = runtimeVersion;
                    output.WriteLine($"Runtime version spec name: {runtimeVersion.SpecName}");
                });

                while (messagesCount < 2 || rv == null)
                {
                    Thread.Sleep(300);
                }

                app.UnsubscribeBlockNumber(subId);
                app.UnsubscribeRuntimeVersion(subId2);
                app.Disconnect();

                Assert.True(blockNum > 0);
                Assert.True(rv != null);
            }
        }
    }
}
