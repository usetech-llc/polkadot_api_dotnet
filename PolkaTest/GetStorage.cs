namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Newtonsoft.Json.Linq;
    using System;
    using Polkadot.DataStructs;
    using Extensions.Data;
    using System.Text;
    using Polkadot.Utils;

    [Collection("Sequential")]
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
            string address = Constants.LocalAliceAddress.Symbols;
            string module1 = "Balances";
            string variable1 = "TotalIssuance";
            string module2 = "System";
            string variable2 = "Account";
            string module3 = "Timestamp";
            string variable3 = "Now";

            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect(Constants.LocalNodeUri);
                output.WriteLine("================== Get Storage 1: Address Balance ==================");
                string balance = app.StorageApi.GetStorage(module1, variable1, new Address(address));
                output.WriteLine($"Encoded balance: {balance}");
                Assert.True(balance != "null");
                Assert.True(balance.Length > 0);

                output.WriteLine("================== Get Storage 2: System Account ==================");
                string accountBalance = app.StorageApi.GetStorage(module2, variable2, new Address(address));
                output.WriteLine($"Encoded account balance: {accountBalance}");
                Assert.True(accountBalance != "null");
                Assert.True(accountBalance.Length > 0);

                output.WriteLine("================== Get Storage 3: Timestamp ==================");
                string timeNow = app.StorageApi.GetStorage(module3, variable3);
                output.WriteLine($"Encoded timestamp: {timeNow}");
                Assert.True(timeNow != "null");
                Assert.True(timeNow.Length > 0);

                output.WriteLine("================== Get Storage 4: Non-existing storage key ==================");
                try
                {
                    string abra = app.StorageApi.GetStorage("Abra", "Cadabra");
                }
                catch (Exception)
                {
                    output.WriteLine("Caught expected ApplicationException");
                }

                app.Disconnect();
            }
        }
    }
}
