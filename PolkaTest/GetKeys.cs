namespace PolkaTest
{
    using Polkadot.Api;
    using System;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using Newtonsoft.Json.Linq;
    using Polkadot.DataStructs;

    [Collection("Sequential")]
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
            using (IApplication app = PolkaApi.GetApplication())
            {
                string address = "5HQdHxuPgQ1BpJasmm5ZzfSk5RDvYiH6YHfDJVE8jXmp4eig";
                string module = "System";
                string variable = "Account";

                app.Connect();

                var actualKey = app.GetKeys(new Address(address), module, variable);

                output.WriteLine($"Storage key for prefix {module} {variable} for address {address} : {actualKey}");
                Console.WriteLine($"\nStorage key for prefix {module} {variable} for address {address} : {actualKey}\n");

                app.Disconnect();
            }
        }
    }
}
