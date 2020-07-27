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
    public class PendingExtrinsic
    {
        private readonly ITestOutputHelper output;

        public PendingExtrinsic(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect();

                var pendingExtrinsics = app.PendingExtrinsics(10);
                foreach(var item in pendingExtrinsics)
                {
                    output.WriteLine($"SignerAddress: {item.SignerAddress}");
                    output.WriteLine($"Signature: {item.Signature}");
                    output.WriteLine($"Method: {item.Method}");
                    output.WriteLine($"Length: {item.Length}");

                    Console.WriteLine($"SignerAddress: {item.SignerAddress}");
                    Console.WriteLine($"Signature: {item.Signature}");
                    Console.WriteLine($"Method: {item.Method}");
                    Console.WriteLine($"Length: {item.Length}");
                }

                app.Disconnect();
            }
        }
    }
}
