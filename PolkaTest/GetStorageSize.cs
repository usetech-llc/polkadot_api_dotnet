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
            string module1 = "System";
            string variable1 = "Account";
            int expectedBalanceSize = 72;

            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage Size: Address Balance ==================");
                var storageSize = app.StorageApi.GetStorageSize(module1, variable1, new Address(Constants.KusamaAccount1Address));
                output.WriteLine($"Storage size: {storageSize}");
                Assert.Equal(expectedBalanceSize, storageSize);
             
                app.Disconnect();
            }
        }
    }
}
