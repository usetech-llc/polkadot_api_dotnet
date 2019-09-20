namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

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

                int subId = app.SubscribeBlockNumber((blockNumber) =>
                {
                    blockNum = blockNumber;
                    messagesCount++;
                });

                while(messagesCount <= 2)
                {
                    Thread.Sleep(100);
                }

                app.UnsubscribeBlockNumber(subId);
                app.Disconnect();

                Assert.True(blockNum > 0);
            }
        }
    }
}
