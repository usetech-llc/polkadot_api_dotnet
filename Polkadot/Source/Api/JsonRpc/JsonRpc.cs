namespace Polkadot.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks.Dataflow;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public struct JsonRpcParams
    {
        public string jsonrpcVersion { get; set; }
    };

    public class JsonRpc : IJsonRpc, IMessageObserver
    {
        private IWebSocketClient _wsc;
        private ILogger _logger;
        private JsonRpcParams _jsonRpcParams;

        private Dictionary<int, BufferBlock<JObject>> _responces;
        private Dictionary<int, BufferBlock<JObject>> _subscriptions;

        private int _lastId = 0;

        private int GetNextId()
        {
            return ++_lastId;
        }

        public JsonRpc(IWebSocketClient wsc, ILogger logger, JsonRpcParams param)
        {
            _wsc = wsc;
            _logger = logger;
            _jsonRpcParams = param;

            _responces = new Dictionary<int, BufferBlock<JObject>>();
            _subscriptions = new Dictionary<int, BufferBlock<JObject>>();
            _wsc.RegisterMessageObserver(this);
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
            var query = new JsonRpcQuery();
            query.Id = GetNextId();

            // build request
            JObject request = JObject.FromObject(
                new {
                    id = query.Id, 
                    jsonrpc = _jsonRpcParams.jsonrpcVersion, 
                    method = jsonMap["method"], 
                    @params = jsonMap["params"]
                }
            );

            // create async receiver
            BufferBlock<JObject> responce;
            bool exists = _responces.TryGetValue(query.Id, out responce);

            if (!exists)
            {
                responce = new BufferBlock<JObject>();
                _responces.Add(query.Id, responce);
            }

            // Send the command
            if (_wsc.IsConnected())
            {
                _wsc.Send(request.ToString());

                string message = $"Message {query.Id} was sent";
                _logger.Info(message);
            }
            else
            {
                string message = "Not connected";
                _logger.Error(message);
                throw new ApplicationException(message);
            }

            var resp = responce.Receive();         
            _responces.Remove(query.Id);

            return resp;
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
            dynamic json = JsonConvert.DeserializeObject(payload);
            _logger.Info($"Message received: {payload}");
            long requestId = 0;
            long subscriptionId = 0;
            if (json["id"] != null)
                requestId = json["id"].Value;
            if (json["params"] != null)
                subscriptionId = json["params"]["subscription"].Value;

            var result = json["result"] as JObject;

            if (result == null)
                result = JObject.FromObject( new { result = json["result"].ToString() });

            _responces.GetValueOrDefault((int)requestId).SendAsync(result);
        }

        public void Dispose()
        {
            _wsc.Dispose();
        }
    }
}