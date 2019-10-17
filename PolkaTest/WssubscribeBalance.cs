namespace PolkaTest
{
    using Polkadot.Api;
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

                string addr = "5HQdHxuPgQ1BpJasmm5ZzfSk5RDvYiH6YHfDJVE8jXmp4eig";
                bool doneS = false;
                BigInteger balanceResult = maxValue;
                var sid = app.SubscribeBalance(addr, (balance) => {
                    output.WriteLine($"Balance: {balance}");
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
