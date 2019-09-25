namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using Newtonsoft.Json.Linq;
    using System;

    public class GetStorage
    {
        private readonly ITestOutputHelper output;

        public GetStorage(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            string address = "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr";
            string module1 = "Balances";
            string variable1 = "FreeBalance";
            string module2 = "System";
            string variable2 = "AccountNonce";
            string module3 = "Timestamp";
            string variable3 = "Now";

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage 1: Address Balance ==================");
                JObject prm1 = new JObject{ { "type", "AccountId"}, { "value", address} };
                string balance = app.GetStorage(prm1.ToString(), module1, variable1);
                output.WriteLine($"Encoded balance: {balance}");
                Assert.True(balance != "null");
                Assert.True(balance.Length > 0);

                output.WriteLine("================== Get Storage 2: Address Nonce ==================");
                string nonce = app.GetStorage(prm1.ToString(), module2, variable2);
                output.WriteLine($"Encoded nonce: {nonce}");
                Assert.True(balance != "null");
                Assert.True(balance.Length > 0);

                output.WriteLine("================== Get Storage 3: Timestamp ==================");
                string timeNow = app.GetStorage(string.Empty, module3, variable3);
                output.WriteLine($"Encoded timestamp: {timeNow}");
                Assert.True(balance != "null");
                Assert.True(balance.Length > 0);

                output.WriteLine("================== Get Storage 4: Non-existing storage key ==================");
                try
                {
                    string abra = app.GetStorage("", "Abra", "Cadabra");
                }
                catch (Exception e)
                {
                    output.WriteLine("Caught expected ApplicationException");
                }

                app.Disconnect();
            }
        }
    }
}
