namespace Polkadot.Test
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Api;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks.Dataflow;

    public class MockJsonRpc : IJsonRpc, IMessageObserver
    {
        private IWebSocketClient _wsc;
        private ILogger _logger;
        private JsonRpcParams _jsonRpcParams;

        protected JObject getRuntimeVersion()
        {
            return JObject.Parse("{result: {\"apis\": [[\"0xdf6acb689907609b\", 2], [\"0x37e397fc7c91f5e4\", 1], " +
                               "[\"0x40fe3ad401f8959a\", 3], [\"0xd2bc9897eed08f15\", 1], [\"0xf78b278be53f454c\", 1], " +
                               "[\"0xaf2c0297a23e6d3d\", 1], [\"0xed99c5acb25eedf5\", 2], [\"0xdd718d5cc53262d4\", 1], " +
                               "[\"0x7801759919ee83e5\", 1]], \"authoringVersion\": 1, \"implName\": \"parity-polkadot\", " +
                               "\"implVersion\": 1, \"specName\": \"polkadot\", \"specVersion\": 112}}");
        }


        public MockJsonRpc(IWebSocketClient wsc, ILogger logger, JsonRpcParams param)
        {
            _wsc = wsc;
            _logger = logger;
            _jsonRpcParams = param;
        }

        public int Connect(string node_url)
        {
            _wsc.Connect(node_url);

            return Consts.PAPI_OK;
        }

        public void Disconnect()
        {
            _wsc.Disconnect();
        }

        public JObject Request(JObject jsonMap, long timeout_s = Consts.RESPONSE_TIMEOUT_S)
        {
            JObject ret = null;

            if (jsonMap["method"].ToString().Equals("chain_getRuntimeVersion"))
            {
                ret = getRuntimeVersion();
            }

            return ret;
        }

        public int SubscribeWs(JObject jsonMap, IWebSocketMessageObserver observer)
        {
            throw new NotImplementedException();
        }

        public int UnsubscribeWs(int subscriptionId, string method)
        {
            throw new NotImplementedException();
        }

        public void HandleMessage(string payload)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _wsc.Dispose();
        }
    }
}
