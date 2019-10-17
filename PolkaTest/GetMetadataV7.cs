namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Newtonsoft.Json.Linq;
    using System;

    [Collection("Sequential")]
    public class GetMetadataV7
    {
        private readonly ITestOutputHelper output;

        public GetMetadataV7(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect("wss://kusama-rpc.polkadot.io/");
                app.Disconnect();
            }
        }
    }
}
