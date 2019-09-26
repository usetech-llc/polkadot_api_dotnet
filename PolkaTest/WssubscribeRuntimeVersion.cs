namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using System.Threading;

    [Collection("Sequential")]
    public class WssubscribeRuntimeVersion
    {
        private readonly ITestOutputHelper output;

        public WssubscribeRuntimeVersion(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                RuntimeVersion rv = null;
                app.Connect();

                int subId = app.SubscribeRuntimeVersion((runtimeVersion) =>
                {
                    rv = runtimeVersion;
                    output.WriteLine($"Runtime version spec name: {runtimeVersion.SpecName}");
                });

                while(rv == null)
                {
                    Thread.Sleep(300);
                }

                app.UnsubscribeRuntimeVersion(subId);
                app.Disconnect();

                Assert.True(rv != null);
            }
        }
    }
}
