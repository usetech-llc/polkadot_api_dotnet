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
            string module1 = "System";
            string variable1 = "Account";

            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage Hash: Address Balance ==================");
                string storageHash = app.StorageApi.GetStorageHash(module1, variable1, new Address(Constants.KusamaAccount1Address));
                output.WriteLine($"Storage hash: {storageHash}");
                Assert.Equal(66, storageHash.Length);
             
                app.Disconnect();
            }
        }
    }
}
