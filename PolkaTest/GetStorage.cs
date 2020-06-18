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
            string address = "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr";
            string address2 = "5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY";
            string module1 = "Balances";
            string variable1 = "TotalIssuance";
            string module2 = "System";
            string variable2 = "Account";
            string module3 = "Timestamp";
            string variable3 = "Now";


            //var aupk = AddressUtils.GetPublicKeyFromAddr("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY");
            //var str1 = BitConverter.ToString(aupk.Bytes).Replace("-", "");

            //var bts = Encoding.ASCII.GetBytes("T::AccountId 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY");
            //var sk = GetStorageKey(Hasher.BLAKE2, aupk.Bytes, aupk.Bytes.Length);

            var a = new Address(address2);
            var t = a.GetTypeEncoded();

            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                //output.WriteLine("================== Get Storage 1: Address Balance ==================");
                // JObject prm1 = new JObject { { "type", "AccountId" }, { "value", address } };
                //string balance = app.GetStorage(prm1.ToString(), module1, variable1);
                //output.WriteLine($"Encoded balance: {balance}");
                //Assert.True(balance != "null");
                //Assert.True(balance.Length > 0);

                output.WriteLine("================== Get Storage 2: System Account ==================");
                // JObject prm2 = new JObject { { "type", "AccountId" }, { "value", address2 } };
                string accountBalance = app.GetStorage(new Address(address), module2, variable2);
                output.WriteLine($"Encoded account balance: {accountBalance}");
                Assert.True(accountBalance != "null");
                Assert.True(accountBalance.Length > 0);

                output.WriteLine("================== Get Storage 3: Timestamp ==================");
                string timeNow = app.GetStorage(module3, variable3);
                output.WriteLine($"Encoded timestamp: {timeNow}");
                Assert.True(timeNow != "null");
                Assert.True(timeNow.Length > 0);

                output.WriteLine("================== Get Storage 4: Non-existing storage key ==================");
                try
                {
                    string abra = app.GetStorage("Abra", "Cadabra");
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
