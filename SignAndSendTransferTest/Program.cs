using Polkadot.Api;
using System;
using System.Numerics;
using System.Threading;

namespace ExtrinsicTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is a manual test due to lack of test DOTs
            if (args.Length < 4)
            {
                Console.WriteLine("This is intended to be a manual test");
                Console.WriteLine("Usage: ");
                Console.WriteLine("<sender address> <recipient address> <amount in fDOTs> <sender private key (hex)>");
                Console.WriteLine("success");
            }
            else
            {
                var senderAddr = args[0];
                var recipientAddr = args[1];
                var amountStr = args[2];
                var senderPrivateKeyStr = args[3];

                using (IApplication app = PolkaApi.GetApplication())
                {
                    bool done = false;

                    var cb = new Action<string>((json) =>
                    {
                        Console.WriteLine(json);
                        done = true;
                    });

                    var amount = BigInteger.Parse(amountStr);

                    app.Connect("ws://127.0.0.1:9944");
                    app.SignAndSendTransfer(senderAddr, senderPrivateKeyStr, recipientAddr, amount, cb);

                    // Wait until transaction is mined
                    while (!done)
                        Thread.Sleep(100);

                    app.Disconnect();
                }
            }
        }
    }
}
