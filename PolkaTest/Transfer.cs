namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Threading;
    using Polkadot.Data;
    using System;
    using System.Numerics;

    [Collection("Sequential")]
    public class Transfer
    {
        private readonly ITestOutputHelper output;

        public Transfer(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                var senderAddr = "5GuuxfuxbvaiwteUrV9U7Mj2Fz7TWK84WhLaZdMMJRvSuzr4";
                var recipientAddr = "5HQdHxuPgQ1BpJasmm5ZzfSk5RDvYiH6YHfDJVE8jXmp4eig";
                var amountStr = "1000000000000";
                var senderPrivateKeyStr = "0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b";
                bool done = false;

                var cb = new Action<string>((json) => {
                    output.WriteLine(json);
                    done = true;
                });

                var amount = BigInteger.Parse(amountStr);

                app.Connect();
                app.SignAndSendTransfer(senderAddr, senderPrivateKeyStr, recipientAddr, amount, cb);

                // Wait until transaction is mined
                while (!done)
                    Thread.Sleep(100);

                app.Disconnect();
            }
        }
    }
}
