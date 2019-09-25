namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using Newtonsoft.Json.Linq;

    public class GetKeys
    {
        private readonly ITestOutputHelper output;

        public GetKeys(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                string address = "5HQdHxuPgQ1BpJasmm5ZzfSk5RDvYiH6YHfDJVE8jXmp4eig";
                string module = "Balances";
                string variable = "FreeBalance";

                app.Connect();

                JObject prm = new JObject{{ "type", "AccountId"}, { "value", address}};
                var actualKey = app.GetKeys(prm.ToString(), module, variable);

                output.WriteLine($"Storage key for prefix {module} {variable} for address {address} : {actualKey}");
                app.Disconnect();
            }
        }
    }
}
