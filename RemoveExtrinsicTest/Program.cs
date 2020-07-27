using Polkadot.Api;
using Polkadot.DataStructs;
using Polkadot.Source.Utils;
using Polkadot.Utils;
using System;
using System.Numerics;
using System.Threading;
using Polkadot.BinarySerializer;

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
                    // Receiving address public key
                    var pk = AddressUtils.GetPublicKeyFromAddr(recipientAddr);

                    // Compact-encode amount
                    var compactAmount = Scale.EncodeCompactInteger(BigInteger.Parse(amountStr));

                    var buf = new byte[pk.Bytes.Length + compactAmount.Bytes.Length];
                    pk.Bytes.CopyTo(buf.AsMemory());
                    compactAmount.Bytes.CopyTo(buf.AsMemory(pk.Bytes.Length));

                    app.Connect();
                    var exHash = app.SubmitExtrinsic(buf, "balances", "transfer", new Address(senderAddr), senderPrivateKeyStr);

                    Console.WriteLine($"Sent extrinsic with hash: {exHash} ");
                    Console.WriteLine("Now let's try to cancel it... ");

                    try
                    {
                        app.RemoveExtrinsic(exHash);
                    }
                    catch (ApplicationException)
                    {
                        Console.WriteLine("Yeah, looks like canceling is not yet supported");
                    }

                    app.Disconnect();
                }
            }
        }
    }
}
