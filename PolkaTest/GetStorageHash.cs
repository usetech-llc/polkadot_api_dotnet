namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Newtonsoft.Json.Linq;

    [Collection("Sequential")]
    public class GetStorageHash
    {
        private readonly ITestOutputHelper output;

        public GetStorageHash(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            string address = "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr";
            string module1 = "Balances";
            string variable1 = "FreeBalance";

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage Hash: Address Balance ==================");
                JObject prm1 = new JObject{ { "type", "AccountId"}, { "value", address} };
                string storageHash = app.GetStorageHash(prm1.ToString(), module1, variable1);
                output.WriteLine($"Storage hash: {storageHash}");
                Assert.True(storageHash.Length == 66);
             
                app.Disconnect();
            }
        }
    }
}
