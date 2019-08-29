using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Polkadot.Source
{
    public struct JsonRpcParams
    {
        public string jsonrpcVersion { get; set; }
    };

    public class JsonRpc : IJsonRpc, IMessageObserver
    {
        


        //private:
        //std::string _jsonrpcVersion;
        //    unsigned int _lastId;
        //    ILogger* _logger;
        //    IWebSocketClient* _wsc;

        //    // Map between request IDs and waiting requests
        //    map<int, JsonRpcQuery> _queries;
        //    mutex _queryMtx;

        //    // Map between subscription IDs and subscribers
        //    map<int, IWebSocketMessageObserver*> _wsSubscribers;

        //    int getNextId();
        //    ConcurrentMapQueue<Json, int> _queue;

        //    public:
        //CJsonRpc(IWebSocketClient* wsc, ILogger* logger, JsonRpcParams params);
        //    ~CJsonRpc() override {}
        //virtual int connect(string node_url = "");
        //virtual void disconnect();
        //virtual Json request(Json jsonMap, long timeout_s = RESPONSE_TIMEOUT_S);
        //virtual void handleMessage(const string &payload);
        //virtual int subscribeWs(Json jsonMap, IWebSocketMessageObserver* observer);
        //virtual int unsubscribeWs(int subscriptionId, string method);

        private IWebSocketClient _wsc;
        private ILogger _logger;
        private JsonRpcParams _jsonRpcParams;
       // private BufferBlock<JsonRpcQuery>[] _queries = new BufferBlock<JsonRpcQuery>[255];
        private BufferBlock<JObject>[] _responces = new BufferBlock<JObject>[255];
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
            //        // Generate new request Id and place request in query map
            //        _queryMtx.lock () ;
            //        condition_variable completionCV; // Condition variable used to notify about response
            //        mutex completionMtx;             // Mutex for condition varaiable
            //        JsonRpcQuery query;
            //        query.id = getNextId();
            //        query.completionCV = &completionCV;
            //        query.completionMtx = &completionMtx;
            //        _queries[query.id] = query;
            //        _queryMtx.unlock();

            //        // build request
            //        Json request = Json::object{
            //            { "id", query.id}, { "jsonrpc", _jsonrpcVersion}, { "method", jsonMap["method"]}, { "params", jsonMap["params"]},
            //};

            //        // Send the command
            //        if (_wsc->isConnected())
            //        {
            //            _wsc->send(request.dump());

            //            string message = "Message " + std::to_string(query.id) + " was sent";
            //            _logger->info(message);
            //        }
            //        else
            //        {
            //            string errstr("Not connected");
            //            _logger->error(errstr);
            //            throw JsonRpcException(errstr);
            //        }

            //        // Block until a timeout happens or response is received
            //        std::unique_lock<std::mutex> responseWaitLock(*query.completionMtx);
            //        query.completionCV->wait_for(responseWaitLock, std::chrono::seconds(timeout_s));

            //        // Move response object and return it
            //        Json result = move(_queries[query.id].json);
            //        _queries.erase(query.id);

            //        return move(result);


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

           // _queries[query.Id].Post(query);

            if (_responces[query.Id] == null)
                _responces[query.Id] = new BufferBlock<JObject>();

            var resp = _responces[query.Id].Receive();

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
            //string err;
            //_logger->info(string("Message received: ") + payload);
            //Json json = Json::parse(payload, err);

            //int requestId = 0;
            //int subscriptionId = 0;
            //if (!json["id"].is_null())
            //    requestId = json["id"].int_value();
            //if (!json["params"].is_null())
            //    subscriptionId = json["params"]["subscription"].int_value();

            //// do not react with health response
            //if (requestId == INT_MAX)
            //    return;

            //if (requestId && _queries.count(requestId))
            //{
            //    // Response for requestId arrived. Set response and notify
            //    _queryMtx.lock () ;
            //    _queries[requestId].json = json["result"];
            //    condition_variable* completionCV = _queries[requestId].completionCV;
            //    _queryMtx.unlock();
            //    completionCV->notify_all();
            //}
            //else if (subscriptionId)
            //{

            //    // Subscription response arrived.
            //    auto result = json["params"]["result"];
            //    _queue.put(subscriptionId, result);

            //}
            //else
            //{
            //    _logger->error("Unknown type of response: " + payload);
            //}

            dynamic json = JsonConvert.DeserializeObject(payload);  //Json::parse(payload, err);
            long requestId = 0;
            long subscriptionId = 0;
            if (json["id"] != null)
                requestId = json["id"].Value;
            if (json["params"] != null)
                subscriptionId = json["params"]["subscription"].Value;

            var result = json["result"] as JObject;

            if (result == null)
                result = JObject.FromObject( new { result = json["result"].ToString() });

            _responces[requestId].SendAsync(result);
        }

        public void Dispose()
        {
            _wsc.Dispose();
        }
    }
}