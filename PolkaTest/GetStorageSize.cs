namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using Newtonsoft.Json.Linq;
    using System;

    public class GetStorageSize
    {
        private readonly ITestOutputHelper output;

        public GetStorageSize(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            string address = "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr";
            string module1 = "Balances";
            string variable1 = "FreeBalance";
            int expectedBalanceSize = 16; // 128 bits

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage Size: Address Balance ==================");
                JObject prm1 = new JObject{ { "type", "AccountId"}, { "value", address} };
                var storageSize = app.GetStorageSize(prm1.ToString(), module1, variable1);
                output.WriteLine($"Storage size: {storageSize}");
                Assert.True(storageSize == expectedBalanceSize);
             
                app.Disconnect();
            }
        }
    }
}
