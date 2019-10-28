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
    public class EraSessionWssubscribe
    {
        private readonly ITestOutputHelper output;

        public EraSessionWssubscribe(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            Thread.Sleep(10000);

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

                output.WriteLine($"--- Alexander ---");
                Console.WriteLine($"--- Alexander ---");
                app.Connect();

                var id = app.SubscribeEraAndSession((era, session) => {

                    if (session.IsEpoch)
                    {
                        output.WriteLine($"Epoch: {session.EpochProgress}  /  {session.EpochLength }");
                        Console.WriteLine($"\nEpoch: {session.EpochProgress}  /  {session.EpochLength }");
                    }
                    else
                    {
                        output.WriteLine($"Session: {session.SessionProgress}  / {session.SessionLength}");
                        Console.WriteLine($"\nSession: {session.SessionProgress}  / {session.SessionLength}");
                    }
                    output.WriteLine($"Era: {era.EraProgress} /  {era.EraLength} ");
                    Console.WriteLine($"Era: {era.EraProgress} /  {era.EraLength} \n");

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
                app.Disconnect();
            }

            Thread.Sleep(10000);

            // Kusama test
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

                output.WriteLine($"--- Kusama ---");
                Console.WriteLine($"--- Kusama ---");
                app.Connect("wss://kusama-rpc.polkadot.io/");

                var id = app.SubscribeEraAndSession((era, session) => {

                    if (session.IsEpoch)
                    {
                        output.WriteLine($"Epoch: {session.EpochProgress}  /  {session.EpochLength }");
                        Console.WriteLine($"\nEpoch: {session.EpochProgress}  /  {session.EpochLength }");
                    }
                    else
                    {
                        output.WriteLine($"Session: {session.SessionProgress}  / {session.SessionLength}");
                        Console.WriteLine($"\nSession: {session.SessionProgress}  / {session.SessionLength}");
                    }
                    output.WriteLine($"Era: {era.EraProgress} /  {era.EraLength} ");
                    Console.WriteLine($"Era: {era.EraProgress} /  {era.EraLength} \n");

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
                app.Disconnect();
            }
        }
    }
}
