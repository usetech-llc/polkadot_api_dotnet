namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Threading;

    [Collection("Sequential")]
    public class WssubscribeBlockNumber
    {
        private readonly ITestOutputHelper output;

        public WssubscribeBlockNumber(ITestOutputHelper output)
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
                app.Connect();

                var subId = app.SubscribeBlockNumber((blockNumber) =>
                {
                    blockNum = blockNumber;
                    messagesCount++;
                    output.WriteLine($"Last block number        : {blockNumber}");
                });

                while (messagesCount < 1)
                {
                    Thread.Sleep(300);
                }

                app.UnsubscribeBlockNumber(subId);

                app.Disconnect();

                Assert.True(blockNum > 0);
            }
        }
    }
}
