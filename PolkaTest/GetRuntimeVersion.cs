namespace PolkaTest
{
    using Polkadot;
    using Polkadot.Api;
    using Polkadot.Data;
    using Polkadot.Test;
    using Xunit;
    using Xunit.Abstractions;

    [Collection("Sequential")]
    public class GetRuntimeVersion
    {
        private readonly ITestOutputHelper output;

        public GetRuntimeVersion(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            JsonRpcParams param = new JsonRpcParams
            {
                JsonrpcVersion = "2.0"
            };

            var logger = new Logger();
            var jsonrpc = new MockJsonRpc(new Wsclient(logger), logger, param);

            using (var _instance = new Application(logger, jsonrpc))
            {
                _instance.Connect();
                var result = _instance.GetRuntimeVersion(
                    new GetRuntimeVersionParams
                    {
                        BlockHash = "0x37096ff58d1831c2ee64b026f8b70afab1942119c022d1dcfdbdc1558ebf63fa"
                    });

                // Ensure all items are present in api
                int apiItemCount = 0;
                foreach (var item in result.Api)
                {
                    if (item.Num.Length == 18)
                    {
                        apiItemCount++;
                    }
                }

                Assert.Equal(9, apiItemCount);

                // Check implName
                Assert.Equal("parity-polkadot", result.ImplName);

                // Check implVersion
                Assert.Equal(1, result.ImplVersion);

                // Check specName
                Assert.Equal("polkadot", result.SpecName);

                // Check specVersion
                Assert.Equal(112, result.SpecVersion);

                _instance.Disconnect();
            }
        }
    }
}
