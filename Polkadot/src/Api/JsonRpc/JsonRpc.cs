namespace Polkadot.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
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
        private readonly Action<Exception> _errorsHandler;
        private ConcurrentDictionary<string, BufferBlock<JObject>> _responses;
        private Dictionary<string, IWebSocketMessageObserver> _subscriptions;

        private Object _subscriptionLock = new Object();
        private Dictionary<string, JObject> _pendingSubscriptionUpdates;
        private Task _healthTask;
        private CancellationTokenSource _cancelletionTokenSource;
        private CancellationToken _cancelletionToken;

        private int _lastId = 0;

        private string GetNextId()
        {
            return (++_lastId).ToString();
        }

        public JsonRpc(IWebSocketClient wsc, ILogger logger, JsonRpcParams param, Action<Exception> errorsHandler = null)
        {
            _wsc = wsc;
            _logger = logger;
            _jsonRpcParams = param;
            _errorsHandler = errorsHandler;

            _responses = new ConcurrentDictionary<string, BufferBlock<JObject>>();
            _subscriptions = new Dictionary<string, IWebSocketMessageObserver>();
            _pendingSubscriptionUpdates = new Dictionary<string, JObject>();
            _wsc.RegisterMessageObserver(this);
        }

        public int Connect(ConnectionParameters connectionParams)
        {
            if (connectionParams is null)
                throw new ArgumentNullException(nameof(connectionParams));

            _wsc.Connect(connectionParams);

            _cancelletionTokenSource = new CancellationTokenSource();
            _cancelletionToken = _cancelletionTokenSource.Token;

            _healthTask = new Task(() => 
            {
                while(!_cancelletionToken.IsCancellationRequested)
                {
                    if (_wsc.IsConnected())
                    {
                        // build health request
                        JObject request = JObject.FromObject(
                            new
                            {
                                id = 0,
                                jsonrpc = _jsonRpcParams.JsonrpcVersion,
                                method = "system_health",
                                @params = new JArray { }
                            }
                        );

                        _logger.Info("Health request");
                        _wsc.Send(request.ToString());
                    }

                    for (var i = 0; i < Consts.HEALTH_REQUEST_TIME_SEC && 
                        !_cancelletionToken.IsCancellationRequested; i++)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }, _cancelletionToken , TaskCreationOptions.LongRunning);

            _healthTask.Start();

            return Consts.PAPI_OK;
        }

        public void Disconnect()
        {
            if (_cancelletionTokenSource != null)
            {
                _cancelletionTokenSource.Cancel();
                _healthTask.Wait();
            }
            _wsc.Disconnect();
        }

        public JObject Request(JObject jsonMap, int timeout_s = Consts.RESPONSE_TIMEOUT_S)
        {
            try
            {
                var query = new JsonRpcQuery
                {
                    Id = GetNextId()
                };

                // build request
                JObject request = JObject.FromObject(
                    new
                    {
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

                var resp = response.Receive(new TimeSpan(0, 0, timeout_s));
                _responses.TryRemove(query.Id, out _);

                return resp;
            }
            catch (Exception ex)
            {
                _errorsHandler?.Invoke(ex);
                throw;
            }
        }

        public string SubscribeWs(JObject jsonMap, IWebSocketMessageObserver observer)
        {
            // Send normal request
            var response = Request(jsonMap);

            // Get response for this request and extract subscription ID
            string subscriptionId = response["result"].ToObject<string>();

            JObject pendingResponse = null;
            lock (_subscriptionLock)
            {
                // Check if there is a pending response for this sibscription ID
                // The subscription handler may only be set at this point if update arrived before
                // we knew the subscription ID. In this case, a pending response is present in the response queue.
                // Handle it immediately.

                _pendingSubscriptionUpdates.TryGetValue(subscriptionId, out pendingResponse);

                // _pendingSubscriptionUpdates.TryRemove(subscriptionId, out pendingResponse);
                _logger.Info($"Pending message processed for {subscriptionId}");

                // Register observer for this subscription ID
                _subscriptions.TryAdd(subscriptionId, observer);

                if (pendingResponse != null)
                    observer.HandleWsMessage(subscriptionId, pendingResponse);
            }

            _logger.Info($"Subscribed with subscription ID: {subscriptionId}");

            return subscriptionId;
        }

        public string UnsubscribeWs(string subscriptionId, string method)
        {
            throw new NotImplementedException();
        }

        public void HandleMessage(string payload)
        {
            try
            {
                dynamic json = JsonConvert.DeserializeObject(payload);
                _logger.Info($"Message received: {payload}");
                string requestId = "0";
                string subscriptionId = null;
                if (json["id"] != null)
                    requestId = json["id"].Value.ToString();
                if (json["params"] != null)
                    subscriptionId = json["params"]["subscription"].Value;

                // message is simple request
                if (requestId != "0")
                {
                    // cut off protocol body
                    BufferBlock<JObject> resp;
                    _responses.TryGetValue(requestId, out resp);
                    JObject result = null;
                    try
                    {
                        result = JObject.FromObject(new {result = json["result"].ToString()});
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"error while processing response: requestId - {requestId};   {json.ToString()}");
                        _errorsHandler?.Invoke(ex);
                    }

                    resp?.SendAsync(result);
                }
                else

                    // message is subscription request
                if (subscriptionId != null)
                {
                    // Subscription response arrived.
                    var result = json["params"] as JObject;

                    IWebSocketMessageObserver existingObserver = null;
                    bool handled = false;
                    _logger.Info("Subscription message recieved");

                    lock (_subscriptionLock)
                    {
                        if (!_subscriptions.TryGetValue(subscriptionId, out existingObserver))
                        {
                            // We may get here if subscription update arrives before we know subscription ID
                            // In this case, observer is not found here, pend this response for this subscription ID

                            _pendingSubscriptionUpdates.TryAdd(subscriptionId, result);
                            _logger.Info($"Message collected for {subscriptionId}");
                            handled = true;
                        }

                        if (existingObserver != null)
                        {
                            existingObserver.HandleWsMessage(subscriptionId, result);
                            _logger.Info($"Message processed for {subscriptionId}");
                            handled = true;
                        }
                    }

                    if (!handled)
                    {
                        _logger.Warning("Subscription message missed!!!");
                    }
                }
            }
            catch (Exception ex)
            {
                _errorsHandler?.Invoke(ex);
                throw;
            }
        }

        public void OnError(Exception exception)
        {
            _errorsHandler?.Invoke(exception);
        }

        public void Dispose()
        {
            if (_healthTask != null)
            {
                if(_healthTask.Status == TaskStatus.Running)
                {
                    _cancelletionTokenSource.Cancel();
                    _healthTask.Wait();               
                    _healthTask.Dispose();
                }
            }

            _cancelletionTokenSource?.Dispose();
            _wsc.Dispose();
        }
    }
}
