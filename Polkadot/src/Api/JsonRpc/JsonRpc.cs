namespace Polkadot.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks.Dataflow;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public struct JsonRpcParams
    {
        public string JsonrpcVersion { get; set; }
    };

    public class JsonRpc : IJsonRpc, IMessageObserver
    {
        private IWebSocketClient _wsc;
        private ILogger _logger;
        private JsonRpcParams _jsonRpcParams;
        private ConcurrentDictionary<int, BufferBlock<JObject>> _responses;
        private ConcurrentDictionary<int, IWebSocketMessageObserver> _subscriptions;

        private Object _subscriptionLock = new Object();
        private ConcurrentDictionary<int, JObject> _pendingSubscriptionUpdates;

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

            _responses = new ConcurrentDictionary<int, BufferBlock<JObject>>();
            _subscriptions = new ConcurrentDictionary<int, IWebSocketMessageObserver>();
            _pendingSubscriptionUpdates = new ConcurrentDictionary<int, JObject>();
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

        public JObject Request(JObject jsonMap, int timeout_s = Consts.RESPONSE_TIMEOUT_S)
        {
            var query = new JsonRpcQuery
            {
                Id = GetNextId()
            };

            // build request
            JObject request = JObject.FromObject(
                new {
                    id = query.Id, 
                    jsonrpc = _jsonRpcParams.JsonrpcVersion, 
                    method = jsonMap["method"], 
                    @params = jsonMap["params"]
                }
            );

            // create async receiver
            bool exists = _responses.TryGetValue(query.Id, out BufferBlock<JObject> response);

            if (!exists)
            {
                response = new BufferBlock<JObject>();
                _responses.TryAdd(query.Id, response);
            }

            // Send the command
            if (_wsc.IsConnected())
            {
                _wsc.Send(request.ToString());

                _logger.Info($"Message body {request.ToString()}");
                _logger.Info($"Message {query.Id} was sent");
            }
            else
            {
                string message = "Not connected";
                _logger.Error(message);
                throw new ApplicationException(message);
            }

            var resp = response.Receive(new TimeSpan( 0, 0, timeout_s));
            _responses.TryRemove(query.Id, out _);

            return resp;
        }

        public int SubscribeWs(JObject jsonMap, IWebSocketMessageObserver observer)
        {
            // Send normal request
            var response = Request(jsonMap);

            // Get response for this request and extract subscription ID
            int subscriptionId = response["result"].ToObject<int>();

            JObject pendingResponse = null;
            lock (_subscriptionLock)
            {
                // Check if there is a pending response for this sibscription ID
                // The subscription handler may only be set at this point if update arrived before
                // we knew the subscription ID. In this case, a pending response is present in the response queue.
                // Handle it immediately.
                _pendingSubscriptionUpdates.TryRemove(subscriptionId, out pendingResponse);

                // Register observer for this subscription ID
                _subscriptions.TryAdd(subscriptionId, observer);
            }

            if (pendingResponse != null)
                observer.HandleWsMessage(subscriptionId, pendingResponse);

            _logger.Info($"Subscribed with subscription ID: {subscriptionId}");

            return subscriptionId;
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
            int subscriptionId = 0;
            if (json["id"] != null)
                requestId = json["id"].Value;
            if (json["params"] != null)
                subscriptionId = (int)json["params"]["subscription"].Value;

            // message is simple request
            if (requestId != 0)
            {
                // cut off protocol body
                BufferBlock<JObject> resp;
                _responses.TryGetValue((int)requestId, out resp);
                resp?.SendAsync(JObject.FromObject(new { result = json["result"].ToString() }));
            }
            else

            // message is subscription request
            if (subscriptionId != 0)
            {
                // Subscription response arrived.
                var result = json["params"] as JObject;

                IWebSocketMessageObserver existingObserver = null;

                lock(_subscriptionLock)
                {
                    if (!_subscriptions.TryGetValue(subscriptionId, out existingObserver))
                    {
                        // We may get here if subscription update arrives before we know subscription ID
                        // In this case, observer is not found here, pend this response for this subscription ID

                        _pendingSubscriptionUpdates.TryAdd(subscriptionId, result);
                    }
                }

                if (existingObserver != null)
                    existingObserver.HandleWsMessage(subscriptionId, result);
            }
        }

        public void Dispose()
        {
            _wsc.Dispose();
        }
    }
}