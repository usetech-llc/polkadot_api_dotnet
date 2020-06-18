namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Newtonsoft.Json.Linq;
    using Polkadot.DataStructs;

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
            string module1 = "System";
            string variable1 = "Account";

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage Hash: Address Balance ==================");
                string storageHash = app.GetStorageHash(new Address(address), module1, variable1);
                output.WriteLine($"Storage hash: {storageHash}");
                Assert.True(storageHash.Length == 66);
             
                app.Disconnect();
            }
        }
    }
}
