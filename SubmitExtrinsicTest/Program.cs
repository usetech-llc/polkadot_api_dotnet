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
            //if (args.Length < 4)
            //{
            //    Console.WriteLine("This is intended to be a manual test");
            //    Console.WriteLine("Usage: ");
            //    Console.WriteLine("<sender address> <recipient address> <amount in fDOTs> <sender private key (hex)>");
            //    Console.WriteLine("success");
            //}
            //else
            //{


                var senderAddr = "5GuuxfuxbvaiwteUrV9U7Mj2Fz7TWK84WhLaZdMMJRvSuzr4";
                var recipientAddr = "5HQdHxuPgQ1BpJasmm5ZzfSk5RDvYiH6YHfDJVE8jXmp4eig";
                var amountStr = "1000000000000";
                var senderPrivateKeyStr = "0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b";

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
                    var result = app.SubmitExtrinsic(buf, "balances", "transfer", new Address(senderAddr), senderPrivateKeyStr);
                    Console.WriteLine(result);
                    app.Disconnect();
                }
       //     }
        }
    }
}
