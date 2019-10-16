namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using System.Threading;
    using Polkadot.Data;
    using System;
    using System.Numerics;
    using Polkadot.Utils;
    using Polkadot.DataStructs;
    using Polkadot.Source.Utils;

    [Collection("Sequential")]
    public class SubmitExtrinsic
    {
        private readonly ITestOutputHelper output;

        public SubmitExtrinsic(ITestOutputHelper output)
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

                // Receiving address public key
                var pk = AddressUtils.GetPublicKeyFromAddr(recipientAddr);

                // Compact-encode amount
                var compactAmount = Scale.EncodeCompactInteger(BigInteger.Parse(amountStr));

                var buf = new byte[pk.Bytes.Length + compactAmount.Bytes.Length];
                pk.Bytes.CopyTo(buf.AsMemory());
                compactAmount.Bytes.CopyTo(buf.AsMemory(pk.Bytes.Length));

                app.Connect();              
                var result = app.SubmitExtrinsic(buf, "balances", "transfer", new Address(senderAddr), senderPrivateKeyStr);
                output.WriteLine(result);
                app.Disconnect();
            }
        }
    }
}
