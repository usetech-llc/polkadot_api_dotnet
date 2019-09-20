namespace Polkadot.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using Polkadot.DataFactory;

    public class Application : IApplication, IWebSocketMessageObserver
    {
        private ILogger _logger;
        private IJsonRpc _jsonRpc;
        private Dictionary<int, BufferBlock<JObject>> _subscriptionData;
        private Dictionary<int, CancellationTokenSource> _subscriptionTokens;

        private T Deserialize<T, C>(JObject json) 
            where C : IParseFactory<T>, new()
        {
            try
            {
                return (new C()).Parse(json);
            }
            catch(Exception e)
            {
                var message = "Cannot deserialize data " + e.Message;
                _logger.Error(message);
                throw new ApplicationException(message);
            }
        }

        public Application(ILogger logger, IJsonRpc jsonRpc)
        {
            _logger = logger;
            _jsonRpc = jsonRpc;
            _subscriptionData = new Dictionary<int, BufferBlock<JObject>>();
            _subscriptionTokens = new Dictionary<int, CancellationTokenSource>();
        }

        public int Connect(string node_url = "")
        {
            int result = Consts.PAPI_OK;

            result = _jsonRpc.Connect(node_url);

            return result;
        }

        public void Disconnect()
        {
            _jsonRpc.Disconnect();
        }

        public void Dispose()
        {
            _jsonRpc.Dispose();
        }

        public SystemInfo GetSystemInfo()
        {
            JObject systemNameQuery = JObject.FromObject( new { method = "system_name", @params = new JArray { } } );
            JObject systemNameJson = _jsonRpc.Request(systemNameQuery);

            JObject systemChainQuery = new JObject { { "method", "system_chain"}, { "params", new JArray{ } } };
            JObject systemChainJson = _jsonRpc.Request(systemChainQuery);

            JObject systemVersionQuery = new JObject { { "method", "system_version"}, { "params", new JArray{ } } };
            JObject systemVersionJson = _jsonRpc.Request(systemVersionQuery);

            JObject systemPropertiesQuery = new JObject { { "method", "system_properties"}, { "params", new JArray { } } };
            JObject systemPropertiesJson = _jsonRpc.Request(systemPropertiesQuery);

            JObject completeJson = JObject.FromObject( new {
                item0 = systemNameJson["result"],
                item1 = systemChainJson["result"],
                item2 = systemVersionJson["result"],
                item3 = systemPropertiesJson });

            return Deserialize<SystemInfo, ParseSystemInfo>(completeJson);
        }

        public BlockHash GetBlockHash(GetBlockHashParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockNumber };

            JObject query = new JObject { { "method", "chain_getBlockHash"}, { "params", prm} };

            JObject response = _jsonRpc.Request(query);

            return Deserialize<BlockHash, ParseBlockHash>(response);
        }

        public RuntimeVersion GetRuntimeVersion(GetRuntimeVersionParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockHash };
            JObject query = new JObject { { "method", "chain_getRuntimeVersion" }, { "params", prm} };

            JObject response = _jsonRpc.Request(query);

            return Deserialize<RuntimeVersion, ParseRuntimeVersion>(response);
        }

        public string GetMetadata(GetMetadataParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockHash };
            JObject query = new JObject { { "method", "state_getMetadata" }, { "params", prm } };

            JObject response = _jsonRpc.Request(query);

            return response.ToString();
        }

        // -------------------------------------------------------------------------------------------------------
        public SignedBlock GetBlock(GetBlockParams param)
        {
            throw new NotImplementedException();
        }

        public BlockHeader GetBlockHeader(GetBlockParams param)
        {
            throw new NotImplementedException();
        }

        public NetworkState GetNetworkState()
        {
            throw new NotImplementedException();
        }

        public FinalHead GetFinalizedHead()
        {
            throw new NotImplementedException();
        }

        public int SubscribeBlockNumber(Action<long> callback)
        {
            JObject subscribeQuery = new JObject { { "method", "chain_subscribeNewHead" }, { "params", new JArray { } } };

            var blockNumberSubscriptionId = _jsonRpc.SubscribeWs(subscribeQuery, this);

            InitSubscription(blockNumberSubscriptionId);  

            Task.Run(() =>
            {
                // check if subscription still active
                while (_subscriptionData.ContainsKey(blockNumberSubscriptionId))
                {
                    var ct = _subscriptionTokens.GetValueOrDefault(blockNumberSubscriptionId);
                    var json = _subscriptionData.GetValueOrDefault(blockNumberSubscriptionId).Receive(ct.Token);
                    var test = json["number"].ToString().Substring(2);
                    var blockNumber = long.Parse(test, NumberStyles.HexNumber);
                    callback(blockNumber);
                }
            });

            return blockNumberSubscriptionId;
        }

        public void UnsubscribeBlockNumber(int id)
        {
            RemoveSubscription(id);
        }

        public void HandleWsMessage(int subscriptionId, JObject message)
        {
            // subscription already init otherwise subscription does not exist
            _subscriptionData.GetValueOrDefault(subscriptionId)?.SendAsync(message);
        }

        private void InitSubscription(int subscriptionId)
        {
            if (subscriptionId != 0 && !_subscriptionData.ContainsKey(subscriptionId))
            {
                // init subscription
                lock (_subscriptionData)
                {
                    _subscriptionTokens.Add(subscriptionId, new CancellationTokenSource());
                    _subscriptionData.Add(subscriptionId, new BufferBlock<JObject>());
                }
            }
        }

        private void RemoveSubscription(int subscriptionId)
        {
            if (_subscriptionData.ContainsKey(subscriptionId))
            {
                lock (_subscriptionData)
                {
                    _subscriptionTokens.GetValueOrDefault(subscriptionId).Cancel();
                    _subscriptionData.Remove(subscriptionId);
                    _subscriptionTokens.Remove(subscriptionId);
                }
            }
        }
    }
}
