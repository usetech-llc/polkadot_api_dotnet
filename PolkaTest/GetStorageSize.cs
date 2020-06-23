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
            int expectedBalanceSize = 69;

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                output.WriteLine("================== Get Storage Size: Address Balance ==================");
                var storageSize = app.GetStorageSize(new Address(Constants.KusamaAddress1), module1, variable1);
                output.WriteLine($"Storage size: {storageSize}");
                Assert.Equal(expectedBalanceSize, storageSize);
             
                app.Disconnect();
            }
        }
    }
}
