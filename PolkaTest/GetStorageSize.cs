namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Newtonsoft.Json.Linq;
    using Polkadot.DataStructs;

    [Collection("Sequential")]
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
            string module1 = "System";
            string variable1 = "Account";
            int expectedBalanceSize = 16; // 128 bits

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage Size: Address Balance ==================");
                var storageSize = app.GetStorageSize(new Address(address), module1, variable1);
                output.WriteLine($"Storage size: {storageSize}");
                Assert.True(storageSize == expectedBalanceSize);
             
                app.Disconnect();
            }
        }
    }
}
