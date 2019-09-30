namespace Polkadot.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using Polkadot.DataFactory;
    using Polkadot.DataFactory.Metadata;
    using Polkadot.DataStructs;
    using Polkadot.DataStructs.Metadata;
    using Polkadot.Source.Utils;

    public class Application : IApplication, IWebSocketMessageObserver
    {
        private ILogger _logger;
        private IJsonRpc _jsonRpc;

        private Object _subscriptionLock = new Object();
        private ConcurrentDictionary<int, JObject> _pendingSubscriptionUpdates = new ConcurrentDictionary<int, JObject>();
        private delegate void UpdateDelegate(JObject update);
        private ConcurrentDictionary<int, UpdateDelegate> _subscriptionHandlers = new ConcurrentDictionary<int, UpdateDelegate>();
        private ProtocolParameters _protocolParams;



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
            _protocolParams = new ProtocolParameters();
        }

        public int Connect(string node_url = "")
        {
            int result = Consts.PAPI_OK;

            // Connect to WS
            result = _jsonRpc.Connect(node_url);

            // Read metadata for head block and initialize protocol parameters
            _protocolParams.Metadata = new Metadata(GetMetadata(null));
            _protocolParams.FreeBalanceHasher = _protocolParams.Metadata.GetFuncHasher("Balances", "FreeBalance");
            _protocolParams.FreeBalancePrefix = "Balances FreeBalance";
            _protocolParams.BalanceModuleIndex = _protocolParams.Metadata.GetModuleIndex("Balances", true);
            _protocolParams.TransferMethodIndex = _protocolParams.Metadata.GetCallMethodIndex(
            _protocolParams.Metadata.GetModuleIndex("Balances", false), "transfer");

            if (_protocolParams.FreeBalanceHasher == Hasher.XXHASH)
                _logger.Info("FreeBalance hash function is xxHash");
            else
                _logger.Info("FreeBalance hash function is Blake2-256");

            _logger.Info($"Balances module index: {_protocolParams.BalanceModuleIndex}" );
            _logger.Info($"Transfer call index: {_protocolParams.TransferMethodIndex}" );

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
                item3 = systemPropertiesJson["result"]
            });

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

        public MetadataBase GetMetadata(GetMetadataParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockHash };
            JObject query = new JObject { { "method", "state_getMetadata" }, { "params", prm } };

            JObject response = _jsonRpc.Request(query);

            return Deserialize<MetadataV4, ParseMetadataV4>(response);
        }

        public SignedBlock GetBlock(GetBlockParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockHash };
            JObject query = new JObject { { "method", "chain_getBlock" }, { "params", prm } };

            JObject response = _jsonRpc.Request(query);

            return Deserialize<SignedBlock, ParseBlock>(response);
        }

        public BlockHeader GetBlockHeader(GetBlockParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockHash };
            JObject query = new JObject { { "method", "chain_getHeader" }, { "params", prm } };

            JObject response = _jsonRpc.Request(query);

            return Deserialize<BlockHeader, ParseBlockHeader>(response);
        }

        public FinalHead GetFinalizedHead()
        {
            JObject query = new JObject { { "method", "chain_getFinalizedHead" }, { "params", new JArray { } } };

            JObject response = _jsonRpc.Request(query);

            return Deserialize<FinalHead, ParseFinalizedHead>(response);
        }

        public string GetKeys(string jsonPrm, string module, string variable)
        {
            // Determine if parameters are required for given module + variable
            // Find the module and variable indexes in metadata
            int moduleIndex = _protocolParams.Metadata.GetModuleIndex(module, false);
            if (moduleIndex == -1)
                throw new ApplicationException("Module not found");
            int variableIndex = _protocolParams.Metadata.GetStorageMethodIndex(moduleIndex, variable);
            if (variableIndex == -1)
                throw new ApplicationException("Variable not found");

            string key;
            if (_protocolParams.Metadata.IsStateVariablePlain(moduleIndex, variableIndex))
            {
                key = _protocolParams.Metadata.GetPlainStorageKey(_protocolParams.FreeBalanceHasher, $"{module} {variable}");
            }
            else
            {
                var param = JsonParse.ParseJsonKeyValuePair(jsonPrm);
                key = _protocolParams.Metadata.GetMappedStorageKey(_protocolParams.FreeBalanceHasher, param, $"{module} {variable}");
            }
            return key;
        }

        public string GetStorage(string jsonPrm, string module, string variable)
        {
            // Get most recent block hash
            var headHash = GetBlockHash(null);

            string key = GetKeys(jsonPrm, module, variable);
            JObject query = new JObject { { "method", "state_getStorage" }, { "params", new JArray { key, headHash.Hash } } };
            JObject response = _jsonRpc.Request(query);

            return response.ToString();
        }

        public string GetStorageHash(string jsonPrm, string module, string variable)
        {
            // Get most recent block hash
            var headHash = GetBlockHash(null);

            string key = GetKeys(jsonPrm, module, variable);
            JObject query = new JObject { { "method", "state_getStorageHash" }, { "params", new JArray { key, headHash.Hash } } };
            JObject response = _jsonRpc.Request(query);

            return response["result"].ToString();
        }

        public int GetStorageSize(string jsonPrm, string module, string variable)
        {
            // Get most recent block hash
            var headHash = GetBlockHash(null);

            string key = GetKeys(jsonPrm, module, variable);
            JObject query = new JObject { { "method", "state_getStorageSize" }, { "params", new JArray { key, headHash.Hash } } };
            JObject response = _jsonRpc.Request(query);

            return response["result"].ToObject<int>();
        }

        public string GetChildKeys(string childStorageKey, string storageKey)
        {
            // Get most recent block hash
            var headHash = GetBlockHash(null);

            JObject query = new JObject { { "method", "state_getChildKeys" },
                                          { "params", new JArray { childStorageKey, storageKey, headHash.Hash } } };
            JObject response = _jsonRpc.Request(query);

            return response.ToString();
        }

        public string GetChildStorage(string childStorageKey, string storageKey)
        {
            // Get most recent block hash
            var headHash = GetBlockHash(null);

            JObject query = new JObject { { "method", "state_getChildStorage" },
                { "params", new JArray { childStorageKey, storageKey, headHash.Hash } } };
            JObject response = _jsonRpc.Request(query);

            return response.ToString();
        }

        public string GetChildStorageHash(string childStorageKey, string storageKey)
        {
            // Get most recent block hash
            var headHash = GetBlockHash(null);

            JObject query = new JObject { { "method", "state_getChildStorageHash" },
                { "params", new JArray { childStorageKey, storageKey, headHash.Hash } } };
            JObject response = _jsonRpc.Request(query);

            return response.ToString();
        }

        public int GetChildStorageSize(string childStorageKey, string storageKey)
        {
            // Get most recent block hash
            var headHash = GetBlockHash(null);

            JObject query = new JObject { { "method", "state_getChildStorageSize" },
                { "params", new JArray { childStorageKey, storageKey, headHash.Hash } } };
            JObject response = _jsonRpc.Request(query);

            return response.ToObject<int>();
        }

        public StorageItem[] QueryStorage(string key, string startHash, string stopHash, int itemCount)
        {
            JObject query = new JObject { { "method", "state_queryStorage" },
                 { "params", new JArray { new JArray(key), startHash, stopHash } } };
            JObject response = _jsonRpc.Request(query, 30);

            var si = new List<StorageItem>();

            int i = 0;
            dynamic values = JsonConvert.DeserializeObject(response["result"].ToString());

            while (i < itemCount && (values.Count > i))
            {
                var item = new StorageItem
                {
                    BlockHash = values[i]["block"].ToString(),
                    Key = values[i]["changes"][0][0].ToString(),
                    Value = values[i]["changes"][0][1].ToString()
                };
                si.Add(item);
                i++;
            }

            return si.ToArray();
        }

        public NetworkState GetNetworkState()
        {
            JObject query = new JObject { { "method", "system_networkState" },
                                          { "params", new JArray { } } };
            JObject response = _jsonRpc.Request(query);

            return Deserialize<NetworkState, ParseNetworkState>(response);
        }

        public PeersInfo GetSystemPeers()
        {
            JObject query = new JObject { { "method", "system_peers" },
                                          { "params", new JArray { } } };
            JObject response = _jsonRpc.Request(query);

            return Deserialize<PeersInfo, ParsePeersInfo>(response);
        }

        public SystemHealth GetSystemHealth()
        {
            JObject query = new JObject { { "method", "system_health" },
                                          { "params", new JArray { } } };
            JObject response = _jsonRpc.Request(query);

            return Deserialize<SystemHealth, ParseSystemHealth>(response);
        }

        public int SubscribeBlockNumber(Action<long> callback)
        {
            JObject subscribeQuery = new JObject { { "method", "chain_subscribeNewHead" }, { "params", new JArray { } } };

            return Subscribe(subscribeQuery, (json) => {
                var test = json["result"]["number"].ToString().Substring(2);
                var blockNumber = long.Parse(test, NumberStyles.HexNumber);
                callback(blockNumber);
            });
        }

        public int SubscribeRuntimeVersion(Action<RuntimeVersion> callback)
        {
            JObject subscribeQuery = new JObject { { "method", "state_subscribeRuntimeVersion" }, { "params", new JArray { } } };

            return Subscribe(subscribeQuery, (json) => {
                var rtv = Deserialize<RuntimeVersion, ParseRuntimeVersion>(json);
                callback(rtv);
            });
        }

        private int Subscribe(JObject subscriptionQuery, UpdateDelegate parseFunc)
        {
            var subscriptionId = _jsonRpc.SubscribeWs(subscriptionQuery, this);

            JObject pendingResponse = null;
            lock (_subscriptionLock)
            {
                _pendingSubscriptionUpdates.TryRemove(subscriptionId, out pendingResponse);
                _subscriptionHandlers.TryAdd(subscriptionId, parseFunc);
            }

            if (pendingResponse != null)
                parseFunc(pendingResponse);

            return subscriptionId;
        }

        public void UnsubscribeBlockNumber(int id)
        {
            RemoveSubscription(id, "chain_unsubscribeNewHead");
        }

        public void UnsubscribeRuntimeVersion(int id)
        {
            RemoveSubscription(id, "state_unsubscribeRuntimeVersion");
        }

        public void HandleWsMessage(int subscriptionId, JObject message)
        {
            // subscription already init otherwise subscription does not exist
            UpdateDelegate handler = null;

            lock (_subscriptionLock)
            {
                if (!_subscriptionHandlers.TryGetValue(subscriptionId, out handler))
                {
                    _pendingSubscriptionUpdates.TryAdd(subscriptionId, message);
                }
            }

            handler?.Invoke(message);
        }

        private void RemoveSubscription(int subscriptionId, string method)
        {
            if (_subscriptionHandlers.ContainsKey(subscriptionId))
            {
                JObject unsubscribeQuery = new JObject { { "method", method }, { "params", new JArray { subscriptionId } } };
                _jsonRpc.Request(unsubscribeQuery);

                lock (_subscriptionHandlers)
                {
                    _subscriptionHandlers.TryRemove(subscriptionId, out _);
                }

                _logger.Info($"Unsubscribed from subscription ID: {subscriptionId}");
            }
        }
    }
}
