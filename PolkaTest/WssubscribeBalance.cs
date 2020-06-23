namespace PolkaTest
{
    using Polkadot.Api;
    using System;
    using Xunit;
    using Xunit.Abstractions;
    using System.Threading;
    using Polkadot.Data;
    using System.Numerics;

    [Collection("Sequential")]
    public class WssubscribeBalance
    {
        private readonly ITestOutputHelper output;

        public WssubscribeBalance(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                BigInteger maxValue = (new BigInteger(1) << 128) - 1;

                string addr = Constants.KusamaAddress1;
                bool doneS = false;
                BigInteger balanceResult = maxValue;
                var sid = app.SubscribeBalance(addr, (balance) => {
                    output.WriteLine($"Balance: {balance}");
                    Console.WriteLine($"\nBalance: {balance}\n");
                    balanceResult = balance;
                    doneS = true;
                });

                while (!doneS)
                {
                    Thread.Sleep(1000);
                }

                app.UnsubscribeBalance(sid);

                app.Disconnect();

                Assert.True(balanceResult < maxValue);
            }
        }
    }
}
