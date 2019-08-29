using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polkadot.Source
{
    public class Application : IApplication
    {
        private ILogger _logger;
        private IJsonRpc _jsonRpc;

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
        }

        public int Connect(string node_url)
        {
            //int result = PAPI_OK;

            //// 1. Connect to WS
            //result = _jsonRpc->connect(node_url);

            //// 2. Read last block hash
            //unique_ptr<GetBlockHashParams> par(new GetBlockHashParams);
            //par->blockNumber = 0;
            //auto genesisHashStr = getBlockHash(move(par));
            //for (int i = 0; i < BLOCK_HASH_SIZE; ++i)
            //{
            //    _protocolPrm.GenesisBlockHash[i] = fromHexByte(genesisHashStr->hash + 2 + i * 2);
            //}

            //// 3. Read metadata for head block and initialize protocol parameters
            //_protocolPrm.metadata = getMetadata(nullptr);
            //_protocolPrm.FreeBalanceHasher = getFuncHasher(_protocolPrm.metadata, string("Balances"), string("FreeBalance"));
            //_protocolPrm.FreeBalancePrefix = "Balances FreeBalance";
            //_protocolPrm.BalanceModuleIndex = getModuleIndex(_protocolPrm.metadata, string("Balances"), true);
            //_protocolPrm.TransferMethodIndex = getCallMethodIndex(
            //    _protocolPrm.metadata, getModuleIndex(_protocolPrm.metadata, string("Balances"), false), string("transfer"));

            //if (_protocolPrm.FreeBalanceHasher == XXHASH)
            //    _logger->info("FreeBalance hash function is xxHash");
            //else
            //    _logger->info("FreeBalance hash function is Blake2-256");

            //_logger->info(string("Balances module index: ") + to_string(_protocolPrm.BalanceModuleIndex));
            //_logger->info(string("Transfer call index: ") + to_string(_protocolPrm.TransferMethodIndex));

            //return result;

            int result = Consts.PAPI_OK;

            result = _jsonRpc.Connect(node_url);

            return result;
        }

        public void Disconnect()
        {
            _jsonRpc.Disconnect();
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

        public void Dispose()
        {
            _jsonRpc.Dispose();
        }
    }
}
