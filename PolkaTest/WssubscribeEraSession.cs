namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Threading;
    using Polkadot.Data;
    using System.Numerics;

    [Collection("Sequential")]
    public class WssubscribeEraSession
    {
        private readonly ITestOutputHelper output;

        public WssubscribeEraSession(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                // Subscribe to block number updates
                bool done = false;
                bool isEpoch = false;
                BigInteger eraLength = -1;
                BigInteger eraProgress = -1;
                BigInteger sessionLength = -1;
                BigInteger sessionProgress = -1;
                BigInteger epochLength = -1;
                BigInteger epochProgress = -1;

                app.Connect();

                var id = app.SubscribeEraAndSession((era, session) => {

                    if (session.IsEpoch)
                    {
                        output.WriteLine($"Epoch: {session.EpochProgress}  /  {session.EpochLength }");
                    }
                    else
                    {
                        output.WriteLine($"Session: {session.SessionProgress}  / {session.SessionLength}");
                    }
                    output.WriteLine($"Era: {era.EraProgress} /  {era.EraLength} ");

                    eraLength = era.EraLength;
                    eraProgress = era.EraProgress;
                    isEpoch = session.IsEpoch;

                    if (session.IsEpoch)
                    {
                        epochLength = session.EpochLength;
                        epochProgress = session.EpochProgress;
                    }
                    else
                    {
                        sessionLength = session.SessionLength;
                        sessionProgress = session.SessionProgress;
                    }

                    done = true;
                });

                // Wait until block number update arrives
                while (!done)
                {
                    Thread.Sleep(200);
                }

                Assert.True(eraLength > 0);
                Assert.True(eraProgress >= 0);

                if (isEpoch)
                {
                    Assert.True(epochLength > 0);
                    Assert.True(epochProgress >= 0);
                }
                else
                {
                    Assert.True(sessionLength > 0);
                    Assert.True(sessionProgress >= 0);
                }

                // Unsubscribe and close connection
                app.UnsubscribeEraAndSession(id);
              
            }
        }
    }
}
