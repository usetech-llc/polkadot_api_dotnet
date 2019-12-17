namespace Polkadot.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Numerics;
    using System.Threading;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using Polkadot.DataFactory;
    using Polkadot.DataFactory.Metadata;
    using Polkadot.DataStructs;
    using Polkadot.DataStructs.Metadata;
    using Polkadot.Source.Utils;
    using Polkadot.src.DataStructs;
    using Polkadot.Utils;
    using Schnorrkel;

    //using Schnorrkel;

    public class Application : IApplication, IWebSocketMessageObserver
    {
        private ILogger _logger;
        private IJsonRpc _jsonRpc;

        private Object _subscriptionLock = new Object();
        private ConcurrentDictionary<int, JObject> _pendingSubscriptionUpdates = new ConcurrentDictionary<int, JObject>();
        private delegate void UpdateDelegate(JObject update);
        private ConcurrentDictionary<int, UpdateDelegate> _subscriptionHandlers = new ConcurrentDictionary<int, UpdateDelegate>();
        private ProtocolParameters _protocolParams;

        // Era/epoch/session subscription storage hashes and data
        private string _storageKeyCurrentEra;
        private string _storageKeySessionsPerEra;
        private string _storageKeyCurrentSessionIndex;
        private string _storageKeyBabeGenesisSlot;
        private string _storageKeyBabeCurrentSlot;
        private BigInteger _babeEpochDuration;
        private BigInteger _sessionsPerEra;
        private string _storageKeyBabeEpochIndex;
        private bool _isEpoch; // True, if epochs should be used instead of sessions

        private T Deserialize<T, C>(JObject json)
            where C : IParseFactory<T>, new()
        {
            try
            {
                return (new C()).Parse(json);
            }
            catch (Exception e)
            {
                var message = "Cannot deserialize data " + e.Message;
                _logger.Error(message);
                throw new ApplicationException(message);
            }
        }

        private bool TryDeserialize<T, C>(JObject json, out T buffer)
                where C : IParseFactory<T>, new()
                where T: new()
        {
            buffer = new T();
            try
            {
                buffer = (new C()).Parse(json);
                return true;
            }
            catch (Exception)
            {
                return false;
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
            _protocolParams.GenesisBlockHash = new byte[Consts.BLOCK_HASH_SIZE];

            GetBlockHashParams par = new GetBlockHashParams();
            par.BlockNumber = 0;
            var genesisHashStr = GetBlockHash(par);
            for (int i = 0; i < Consts.BLOCK_HASH_SIZE; ++i)
            {
                _protocolParams.GenesisBlockHash[i] = Converters.FromHexByte(genesisHashStr.Hash.Substring(2 + i * 2));
            }

            // Read metadata for head block and initialize protocol parameters
            _protocolParams.Metadata = new Metadata(GetMetadata(null));
            _protocolParams.FreeBalanceHasher = _protocolParams.Metadata.GetFuncHasher("Balances", "FreeBalance");
            _protocolParams.FreeBalancePrefix = "Balances FreeBalance";
            _protocolParams.BalanceModuleIndex = (byte)_protocolParams.Metadata.GetModuleIndex("Balances", true);
            _protocolParams.TransferMethodIndex = (byte)_protocolParams.Metadata.GetCallMethodIndex(
            _protocolParams.Metadata.GetModuleIndex("Balances", false), "transfer");

            if (_protocolParams.FreeBalanceHasher == Hasher.XXHASH)
                _logger.Info("FreeBalance hash function is xxHash");
            else
                _logger.Info("FreeBalance hash function is Blake2-256");

            _logger.Info($"Balances module index: {_protocolParams.BalanceModuleIndex}");
            _logger.Info($"Transfer call index: {_protocolParams.TransferMethodIndex}");

            // Calculate storage hashes
            _storageKeyCurrentEra = _protocolParams.Metadata.GetPlainStorageKey(_protocolParams.FreeBalanceHasher, "Staking CurrentEra");

            _storageKeySessionsPerEra =
                _protocolParams.Metadata.GetPlainStorageKey(_protocolParams.FreeBalanceHasher, "Staking SessionsPerEra");

            _storageKeyCurrentSessionIndex =
                _protocolParams.Metadata.GetPlainStorageKey(_protocolParams.FreeBalanceHasher, "Session CurrentIndex");

            try
            {
                _storageKeyBabeGenesisSlot = GetKeys("", "Babe", "GenesisSlot");
                _storageKeyBabeCurrentSlot = GetKeys("", "Babe", "CurrentSlot");
                _storageKeyBabeEpochIndex = GetKeys("", "Babe", "EpochIndex");
                _sessionsPerEra = _protocolParams.Metadata.GetConst("Staking", "SessionsPerEra");
                _babeEpochDuration = _protocolParams.Metadata.GetConst("Babe", "EpochDuration");
            }
            catch (ApplicationException)
            {
                // Expected exception if Babe module is not present (e.g. Alexander network)
            }

            // Detect if epochs or sessions should be used
            _isEpoch = _storageKeyBabeGenesisSlot != null;
            if (_isEpoch)
                _logger.Info("Using epochs");
            else
                _logger.Info("Using sessions");


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
            JObject systemNameQuery = JObject.FromObject(new { method = "system_name", @params = new JArray { } });
            JObject systemNameJson = _jsonRpc.Request(systemNameQuery);

            JObject systemChainQuery = new JObject { { "method", "system_chain" }, { "params", new JArray { } } };
            JObject systemChainJson = _jsonRpc.Request(systemChainQuery);

            JObject systemVersionQuery = new JObject { { "method", "system_version" }, { "params", new JArray { } } };
            JObject systemVersionJson = _jsonRpc.Request(systemVersionQuery);

            JObject systemPropertiesQuery = new JObject { { "method", "system_properties" }, { "params", new JArray { } } };
            JObject systemPropertiesJson = _jsonRpc.Request(systemPropertiesQuery);

            JObject completeJson = JObject.FromObject(new
            {
                item0 = systemNameJson["result"],
                item1 = systemChainJson["result"],
                item2 = systemVersionJson["result"],
                item3 = systemPropertiesJson["result"]
            });

            return Deserialize<SystemInfo, ParseSystemInfo>(completeJson);
        }

        public BigInteger GetAccountNonce(Address address)
        {
            // Subscribe to account nonce updates and immediately unsubscribe after response is received
            // This pattern is borrowed from POC-3 UI
            bool done = false;
            BigInteger result = 0;

            var subId = SubscribeAccountNonce(address, (nonce) =>
            {
                result = nonce;
                done = true;
            });

            SpinWait.SpinUntil(() => !done);
            UnsubscribeAccountNonce(subId);

            return result;
        }

        public BlockHash GetBlockHash(GetBlockHashParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockNumber };

            JObject query = new JObject { { "method", "chain_getBlockHash" }, { "params", prm } };

            JObject response = _jsonRpc.Request(query);

            return Deserialize<BlockHash, ParseBlockHash>(response);
        }

        public RuntimeVersion GetRuntimeVersion(GetRuntimeVersionParams param)
        {
            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockHash };
            JObject query = new JObject { { "method", "chain_getRuntimeVersion" }, { "params", prm } };

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

            if (TryDeserialize<MetadataV4, ParseMetadataV4>(response, out MetadataV4 md4))
                return md4;

            if (TryDeserialize<MetadataV7, ParseMetadataV7>(response, out MetadataV7 md7))
                return md7;

            if (TryDeserialize<MetadataV8, ParseMetadataV8>(response, out MetadataV8 md8))
                return md8;

            return null;
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

            return Subscribe(subscribeQuery, (json) =>
            {
                var test = json["result"]["number"].ToString().Substring(2);
                var blockNumber = long.Parse(test, NumberStyles.HexNumber);
                callback(blockNumber);
            });
        }

        public int SubscribeRuntimeVersion(Action<RuntimeVersion> callback)
        {
            JObject subscribeQuery = new JObject { { "method", "state_subscribeRuntimeVersion" }, { "params", new JArray { } } };

            return Subscribe(subscribeQuery, (json) =>
            {
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

        public void UnsubscribeAccountNonce(int id)
        {
            RemoveSubscription(id, "state_unsubscribeStorage");
        }

        public void UnsubscribeEraAndSession(int id)
        {
            RemoveSubscription(id, "state_unsubscribeStorage");
        }

        public void UnsubscribeBalance(int id)
        {
            RemoveSubscription(id, "state_unsubscribeStorage");
        }

        public void UnsubscribeStorage(int id)
        {
            RemoveSubscription(id, "state_unsubscribeStorage");
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

        public int SignAndSendTransfer(string sender, string privateKey, string recipient, BigInteger amount, Action<string> callback)
        {
            _logger.Info("=== Starting a Transfer Extrinsic ===");

            // Get account Nonce
            var address = new Address { Symbols = sender };
            var nonce = GetAccountNonce(address);
            _logger.Info($"sender nonce: {nonce} ");

            // Format transaction
            TransferExtrinsic te = new TransferExtrinsic();
            te.Method.ModuleIndex = _protocolParams.BalanceModuleIndex;
            te.Method.MethodIndex = _protocolParams.TransferMethodIndex;

            var recipientPK = _protocolParams.Metadata.GetPublicKeyFromAddr(new Address(recipient));
            te.Method.ReceiverPublicKey = recipientPK.Bytes;
            te.Method.Amount = amount;
            te.Signature.Version = Consts.SIGNATURE_VERSION;
            var senderPK = _protocolParams.Metadata.GetPublicKeyFromAddr(new Address(sender));
            te.Signature.SignerPublicKey = senderPK.Bytes;
            te.Signature.Nonce = nonce;
            te.Signature.Era = ExtrinsicEra.IMMORTAL_ERA;

            // Format signature payload
            SignaturePayload sp = new SignaturePayload();
            sp.Nonce = nonce;
            var methodBytes = new byte[Consts.MAX_METHOD_BYTES_SZ];
            sp.MethodBytesLength = (int)te.SerializeMethodBinary(ref methodBytes);
            sp.MethodBytes = methodBytes;
            sp.Era = ExtrinsicEra.IMMORTAL_ERA;
            sp.AuthoringBlockHash = _protocolParams.GenesisBlockHash;

            // Serialize and Sign payload
            var signaturePayloadBytes = new byte[Consts.MAX_METHOD_BYTES_SZ];
            long payloadLength = sp.SerializeBinary(ref signaturePayloadBytes);

            byte[] secretKeyVec = Converters.StringToByteArray(privateKey);

            var message = signaturePayloadBytes.AsMemory().Slice(0, (int)payloadLength).ToArray();
            var sig = Sr25519v011.SignSimple(te.Signature.SignerPublicKey, secretKeyVec, message);
            te.Signature.Sr25519Signature = sig;

            // Serialize and send transaction
            var teBytes = new byte[Consts.MAX_METHOD_BYTES_SZ];
            long teByteLength = te.SerializeBinary(ref teBytes);
            string teStr = $"0x{Converters.ByteArrayToString(teBytes, (int)teByteLength)}";

            var query = new JObject { { "method", "author_submitAndWatchExtrinsic" }, { "params", new JArray { teStr } } };

            // Send == Subscribe callback to completion
            return Subscribe(query, (json) =>
            {
                callback(json.ToString());
            });
        }

        public GenericExtrinsic[] PendingExtrinsics(int bufferSize)
        {
            var query = new JObject { { "method", "author_pendingExtrinsics" }, { "params", new JArray { } } };
            JObject response = _jsonRpc.Request(query);

            int count = 0;
            GenericExtrinsic[] genericExtrinsic = new GenericExtrinsic[bufferSize];

            var items = JsonConvert.DeserializeObject(response["result"].ToString()) as JArray;

            foreach (var e in items.Values())
            {
                if (count < genericExtrinsic.Length)
                {
                    genericExtrinsic[count] = new GenericExtrinsic();

                    string estr = e.ToString().Substring(2);
                    genericExtrinsic[count].Length = (ulong)Scale.DecodeCompactInteger(ref estr).Value;

                    // Signature version
                    genericExtrinsic[count].Signature.Version = (byte)Converters.FromHex(estr.Substring(0, 2));
                    estr = estr.Substring(2);

                    // Signer public key
                    var pk = Converters.FromHex(estr.Substring(2, Consts.SR25519_PUBLIC_SIZE * 2));
                    genericExtrinsic[count].Signature.SignerPublicKey = pk.ToByteArray();
                    estr = estr.Substring(Consts.SR25519_PUBLIC_SIZE * 2 + 2);

                    // Signature
                    var sig = Converters.FromHex(estr.Substring(0, Consts.SR25519_SIGNATURE_SIZE * 2));
                    genericExtrinsic[count].Signature.Sr25519Signature = sig.ToByteArray();
                    estr = estr.Substring(Consts.SR25519_SIGNATURE_SIZE * 2);

                    // nonce
                    genericExtrinsic[count].Signature.Nonce = Scale.DecodeCompactInteger(ref estr).Value;

                    // Era
                    byte eraInt = (byte)Converters.FromHex(estr.Substring(0, 2));
                    if (eraInt != 0)
                        genericExtrinsic[count].Signature.Era = ExtrinsicEra.MORTAL_ERA;
                    else
                        genericExtrinsic[count].Signature.Era = ExtrinsicEra.IMMORTAL_ERA;
                    estr = estr.Substring(2);

                    // Method - module index
                    genericExtrinsic[count].Method.ModuleIndex = (byte)Converters.FromHex(estr.Substring(0, 2));
                    estr = estr.Substring(2);

                    // Method - call index
                    genericExtrinsic[count].Method.MethodIndex = (byte)Converters.FromHex(estr.Substring(0, 2));
                    estr = estr.Substring(2);

                    // Method - bytes
                    genericExtrinsic[count].Method.MethodBytes = estr;

                    // Encode signer address to base58
                    PublicKey pubk = new PublicKey();
                    pubk.Bytes = genericExtrinsic[count].Signature.SignerPublicKey;
                    genericExtrinsic[count].SignerAddress = _protocolParams.Metadata.GetAddrFromPublicKey(pubk);
                }
                count++;
            }

            return genericExtrinsic;
        }

        private string ExtrinsicQueryString(byte[] encodedMethodBytes, string module, string method, Address sender, string privateKey)
        {
            _logger.Info("=== Started Invoking Extrinsic ===");

            // Get account Nonce
            var nonce = GetAccountNonce(sender);
            var compactNonce = Scale.EncodeCompactInteger(nonce);
            _logger.Info($"sender nonce: {nonce}");

            byte[] mmBuf = new byte[3];
            // Module + Method
            var absoluteIndex = _protocolParams.Metadata.GetModuleIndex(module, false);
            mmBuf[0] = (byte)_protocolParams.Metadata.GetModuleIndex(module, true);
            mmBuf[1] = (byte)_protocolParams.Metadata.GetCallMethodIndex(absoluteIndex, method);

            // Address separator
            mmBuf[2] = Consts.ADDRESS_SEPARATOR;

            Extrinsic ce = new Extrinsic();

            var completeMessage = new byte[encodedMethodBytes.Length + 3];
            mmBuf.CopyTo(completeMessage.AsMemory());
            encodedMethodBytes.CopyTo(completeMessage.AsMemory(3));

            // memcpy(completeMessage + 3, encodedMethodBytes, encodedMethodBytesSize);

            ce.Signature.Version = Consts.SIGNATURE_VERSION;
            var senderPK = _protocolParams.Metadata.GetPublicKeyFromAddr(sender);
            ce.Signature.SignerPublicKey = senderPK.Bytes;
            ce.Signature.Nonce = nonce;
            ce.Signature.Era = ExtrinsicEra.IMMORTAL_ERA;

            // Format signature payload
            SignaturePayload sp = new SignaturePayload();
            sp.Nonce = nonce;

            sp.MethodBytesLength = encodedMethodBytes.Length + 3;
            sp.MethodBytes = completeMessage;
            sp.Era = ExtrinsicEra.IMMORTAL_ERA;
            sp.AuthoringBlockHash = _protocolParams.GenesisBlockHash;

            // Serialize and Sign payload
            var signaturePayloadBytes = new byte[Consts.MAX_METHOD_BYTES_SZ];
            long payloadLength = sp.SerializeBinary(ref signaturePayloadBytes);

            var secretKeyVec = Converters.StringToByteArray(privateKey);

            var message = signaturePayloadBytes.AsMemory().Slice(0, (int)payloadLength).ToArray();
            var sig = Sr25519v011.SignSimple(ce.Signature.SignerPublicKey, secretKeyVec, message);
            ce.Signature.Sr25519Signature = sig;

            // Copy signature bytes to transaction
            ce.Signature.Sr25519Signature = sig;
            var length = Consts.DEFAULT_FIXED_EXSTRINSIC_SIZE + encodedMethodBytes.Length + compactNonce.Length - 1;
            var compactLength = Scale.EncodeCompactInteger(length);

            /////////////////////////////////////////
            // Serialize message signature and write to buffer

            long writtenLength = 0;
            var buf = new byte[2048];
            var buf2 = new List<byte>();

            // Length
            writtenLength += Scale.WriteCompactToBuf(compactLength, ref buf, writtenLength);

            // Signature version
            buf[writtenLength++] = ce.Signature.Version;

            // Address separator
            buf[writtenLength++] = Consts.ADDRESS_SEPARATOR;

            // Signer public key
            ce.Signature.SignerPublicKey.CopyTo(buf.AsMemory((int)writtenLength));

            writtenLength += Consts.SR25519_PUBLIC_SIZE;

            // SR25519 Signature
            ce.Signature.Sr25519Signature.CopyTo(buf.AsMemory((int)writtenLength));
            writtenLength += Consts.SR25519_SIGNATURE_SIZE;

            // Nonce
            writtenLength += Scale.WriteCompactToBuf(compactNonce, ref buf, writtenLength);

            // Extrinsic Era
            buf[writtenLength++] = (byte)ce.Signature.Era;

            // Serialize and send transaction
            var teBytes = new byte[Consts.MAX_METHOD_BYTES_SZ];
            teBytes = buf;
            completeMessage.AsMemory().CopyTo(teBytes.AsMemory((int)writtenLength));

            long teByteLength = writtenLength + encodedMethodBytes.Length + 3;
            return $"0x{Converters.ByteArrayToString(teBytes, (int)teByteLength)}";
        }

        public string SubmitExtrinsic(byte[] encodedMethodBytes, string module, string method, Address sender, string privateKey)
        {
            string teStr = ExtrinsicQueryString(encodedMethodBytes, module, method, sender, privateKey);

            var query = new JObject { { "method", "author_submitExtrinsic" }, { "params", new JArray { teStr } } };
            var response = _jsonRpc.Request(query);

            return response.ToString();
        }

        public int SubmitAndSubcribeExtrinsic(byte[] encodedMethodBytes, string module, string method, Address sender, string privateKey, Action<string> callback)
        {
            string teStr = ExtrinsicQueryString(encodedMethodBytes, module, method, sender, privateKey);

            var query = new JObject { { "method", "author_submitAndWatchExtrinsic" }, { "params", new JArray { teStr } } };

            // Send == Subscribe callback to completion
            return Subscribe(query, (json) =>
            {
                callback(json.ToString());
            });
        }

        public bool RemoveExtrinsic(string extrinsicHash)
        {
            var query = new JObject { { "method", "author_removeExtrinsic" }, { "params", new JArray { extrinsicHash } } };
            var response = _jsonRpc.Request(query);

            if (response == null)
                throw new ApplicationException("Not supported");

            return false;
        }

        public int SubscribeAccountNonce(Address address, Action<BigInteger> callback)
        {
            var storageKey = _protocolParams.Metadata.GetAddressStorageKey(_protocolParams.FreeBalanceHasher,
                address, "System AccountNonce");

            _logger.Info($"Nonce subscription storageKey: {storageKey}");

            JObject subscribeQuery = new JObject { { "method", "state_subscribeStorage" }, { "params", new JArray { new JArray { storageKey } } } };

            return Subscribe(subscribeQuery, (json) =>
            {
                var result = Converters.FromHex(json["result"]["changes"][0][1].ToString().Substring(2));
                callback(result);
            });
        }

        public int SubscribeStorage(string key, Action<string> callback)
        {
            // Subscribe to websocket
            var subscribeQuery =
                    new JObject { { "method", "state_subscribeStorage" }, { "params", new JArray { new JArray { key } } } };

            return Subscribe(subscribeQuery, (json) =>
            {
                callback(json["result"]["changes"][0][1].ToString());
            });
        }

        public int SubscribeEraAndSession(Action<Era, SessionOrEpoch> callback)
        {
            long _bestBlockNum = 0;
            bool done = false;
            var sbnId = SubscribeBlockNumber(new Action<long>((blockNum) =>
            {
                _bestBlockNum = blockNum;
                done = true;
            }));

            while (!done)
            {
                Thread.SpinWait(1500);
            }

            // era and session subscription
            JArray _params;
            if (_isEpoch)
            {
                _params =
                    new JArray { new JArray { _storageKeyBabeEpochIndex, _storageKeyBabeGenesisSlot, _storageKeyBabeCurrentSlot } };
            }
            else
            {
                _params =
                    new JArray {
                            new JArray {
                                Consts.LAST_LENGTH_CHANGE_SUBSCRIPTION, Consts.SESSION_LENGTH_SUBSCRIPTION, _storageKeyCurrentEra,
                                            _storageKeySessionsPerEra, _storageKeyCurrentSessionIndex}
                        };
            }

            // Subscribe to websocket
            JObject subscribeQuery = new JObject { { "method", "state_subscribeStorage" }, { "params", _params } };

            return Subscribe(subscribeQuery, (json) =>
            {
                var message = json["result"];

                if (_isEpoch)
                {
                    BigInteger epochIndex = 0, epochOrGenesisStartSlot = 0, currentSlot = 0;

                    if (message["changes"][0][1] != null)
                        epochIndex = Converters.FromHex(message["changes"][0][1].ToString(), false);
                    if (message["changes"][1][1] != null)
                        epochOrGenesisStartSlot = Converters.FromHex(message["changes"][1][1].ToString(), false);
                    if (message["changes"][2][1] != null)
                        currentSlot = Converters.FromHex(message["changes"][2][1].ToString(), false);

                    var epochStartSlot = _babeEpochDuration * epochIndex + epochOrGenesisStartSlot;
                    var sessionProgress = currentSlot - epochStartSlot;
                    var eraProgress = epochIndex % _sessionsPerEra * _babeEpochDuration + sessionProgress;
                    var eraDuration = _sessionsPerEra * _babeEpochDuration;

                    Era era;
                    era.EraProgress = eraProgress;
                    era.EraLength = _sessionsPerEra * _babeEpochDuration;
                    SessionOrEpoch session = new SessionOrEpoch
                    {
                        IsEpoch = _isEpoch,
                        EpochProgress = sessionProgress,
                        EpochLength = _babeEpochDuration
                    };

                    callback(era, session);

                }
                else
                {
                    BigInteger lastLengthChange = 0, sessionLength = 0, currentEra = 0, sessionsPerEra = 0,
                              currentIndexSubcription = 0;

                    if (message["changes"][0][1] != null)
                        lastLengthChange = Converters.FromHex(message["changes"][0][1].ToString(), false);
                    if (message["changes"][1][1] != null)
                        sessionLength = Converters.FromHex(message["changes"][1][1].ToString(), false);
                    if (message["changes"][2][1] != null)
                        currentEra = Converters.FromHex(message["changes"][2][1].ToString(), false);
                    if (message["changes"][3][1] != null)
                        sessionsPerEra = Converters.FromHex(message["changes"][3][1].ToString(), false);
                    if (message["changes"][4][1] != null)
                        currentIndexSubcription = Converters.FromHex(message["changes"][4][1].ToString(), false);

                    if (lastLengthChange > 0 && sessionLength > 0 && currentEra > 0 && sessionsPerEra > 0 &&
                        currentIndexSubcription > 0)
                    {
                        var sessionProgress = (_bestBlockNum - lastLengthChange + sessionLength) % sessionLength;
                        var eraProgress = currentIndexSubcription % sessionsPerEra * sessionLength + sessionProgress;

                        Era era;
                        era.EraProgress = eraProgress;
                        era.EraLength = sessionsPerEra * sessionLength;
                        SessionOrEpoch session = new SessionOrEpoch
                        {
                            IsEpoch = _isEpoch,
                            SessionProgress = sessionProgress,
                            SessionLength = sessionLength
                        };

                        callback(era, session);
                    }
                }

                UnsubscribeBlockNumber(sbnId);
            });
        }

        public int SubscribeBalance(string address, Action<BigInteger> callback)
        {
            string storageKey =
                       _protocolParams.Metadata.GetAddressStorageKey(_protocolParams.FreeBalanceHasher,
                       new Address(address), "Balances FreeBalance");

            var subscribeQuery =
                new JObject { { "method", "state_subscribeStorage"}, { "params", new JArray { new JArray { storageKey} } } };

            return Subscribe(subscribeQuery, (json) =>
            {
                callback(BigInteger.Parse(json["result"]["changes"][0][1].ToString().Substring(2), NumberStyles.HexNumber));
            });
        }
    }
}
