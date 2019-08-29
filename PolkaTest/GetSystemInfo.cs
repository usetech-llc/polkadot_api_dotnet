using Polkadot.Source;
using System;
using Xunit;

namespace PolkaTest
{
    public class GetSystemInfo
    {
        [Fact]
        public void Ok()
        {
            JsonRpcParams param = new JsonRpcParams();
            param.jsonrpcVersion = "2.0";

            var logger = new Logger();
            var jsonrpc = new JsonRpc(new Wsclient(logger), logger, param);
            using (IApplication app = new Application(logger, jsonrpc))
            {
                app.Connect();
                var result = app.GetSystemInfo();

                Assert.True(result.chainId.Length > 0);

                // Check chainName
                Assert.Equal("parity-polkadot", result.chainName);

                // Check version
                Assert.NotEqual(string.Empty, result.version);

                // Check tokenSymbol
                Assert.Equal("DOT", result.tokenSymbol);

                // Check tokenDecimals
                Assert.Equal(15, result.tokenDecimals);

                app.Disconnect();
            }
        }
    }
}
