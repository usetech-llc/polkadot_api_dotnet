using Polkadot.Api;
using Polkadot.DataStructs;
using Polkadot.Source.Utils;
using Polkadot.Utils;
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

                using (IApplication app = PolkaApi.GetAppication())
                {

                    // Submit extrinsic and watch
                    bool done = false;

                    // Receiving address public key
                    var pk = AddressUtils.GetPublicKeyFromAddr(recipientAddr);

                    // Compact-encode amount
                    var compactAmount = Scale.EncodeCompactInteger(BigInteger.Parse(amountStr));

                    var buf = new byte[pk.Bytes.Length + compactAmount.Bytes.Length];
                    pk.Bytes.CopyTo(buf.AsMemory());
                    compactAmount.Bytes.CopyTo(buf.AsMemory(pk.Bytes.Length));

                    var callback = new Action<string>((str) => {
                        Console.WriteLine($"Response json:  {str}");
                        done = true;
                    });

                    app.Connect();
                    var result = app.SubmitAndSubcribeExtrinsic(buf, "balances", "transfer", new Address(senderAddr), senderPrivateKeyStr, callback);

                    while (!done)
                    {
                        Thread.SpinWait(500);
                    }

                    app.Disconnect();
                }
            }
        }
    }
}
