namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Threading;
    using Polkadot.Data;

    [Collection("Sequential")]
    public class WssubscribeStorage
    {
        private readonly ITestOutputHelper output;

        public WssubscribeStorage(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();

                // Subscribe to storage updates (timestamp)
                var module = "Timestamp";
                var variable = "Now";
                var key = app.GetKeys("", module, variable);
                bool doneS = false;
                string tsUpdate = string.Empty;
                var sid = app.SubscribeStorage(key, (update) => {
                    output.WriteLine($"Timestamp update: {update}");
                    tsUpdate = update;
                    doneS = true;
                });

                while (!doneS)
                {
                    Thread.SpinWait(1000);
                }

                app.UnsubscribeStorage(sid);

                app.Disconnect();

                Assert.True(tsUpdate.Length > 0);
                Assert.True(tsUpdate.Substring(0, 2) == "0x");
            }
        }
    }
}
