using System.Linq;
using System.Threading.Tasks;
using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs.Metadata.Interfaces;
using Polkadot.Exceptions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polkadot.Api.Hashers;
using Polkadot.BinaryContracts.Events;
using Polkadot.BinaryContracts.Events.Balances;
using Polkadot.BinaryContracts.Events.Contracts;
using Polkadot.BinaryContracts.Events.Grandpa;
using Polkadot.BinaryContracts.Events.Sudo;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.Data;
using Polkadot.DataFactory;
using Polkadot.DataFactory.Metadata;
using Polkadot.DataStructs;
using Polkadot.DataStructs.Metadata;
using Polkadot.DataStructs.Metadata.BinaryContracts;
using Polkadot.src.DataStructs;
using Polkadot.Utils;
using Schnorrkel;
using SignaturePayload = Polkadot.BinaryContracts.SignaturePayload;
using Transfer = Polkadot.BinaryContracts.Events.Balances.Transfer;

namespace Polkadot.Api
{
    public class Application : IApplication, IWebSocketMessageObserver
    {
        private ILogger _logger;
        private IJsonRpc _jsonRpc;
        private readonly SerializerSettings _serializerSettings;

        private MetadataBase _metadataCache;
        private GetMetadataParams _metadataDefaultBlockHash;

        private Object _subscriptionLock = new Object();
        private ConcurrentDictionary<string, JObject> _pendingSubscriptionUpdates = new ConcurrentDictionary<string, JObject>();
        private ConcurrentDictionary<string, Action<JObject>> _subscriptionHandlers = new ConcurrentDictionary<string, Action<JObject>>();
        public ProtocolParameters _protocolParams;

        // Era/epoch/session subscription storage hashes and data
        private string _storageKeySessionsPerEra;
        private string _storageKeyCurrentSessionIndex;
        private string _storageKeyBabeGenesisSlot;
        private string _storageKeyBabeCurrentSlot;
        private BigInteger _babeEpochDuration;
        private BigInteger _sessionsPerEra;
        private string _storageKeyBabeEpochIndex;
        private bool _isEpoch; // True, if epochs should be used instead of sessions
        private Lazy<IBinarySerializer> _serializer;

        public TimeSpan RequestsTimeout { get; set; } = TimeSpan.FromSeconds(Consts.RESPONSE_TIMEOUT_S);

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

        public Application(ILogger logger, IJsonRpc jsonRpc, SerializerSettings serializerSettings)
        {
            _logger = logger;
            _jsonRpc = jsonRpc;
            _serializerSettings = serializerSettings;
            _protocolParams = new ProtocolParameters();
            _serializer = new Lazy<IBinarySerializer>(CreateSerializer);
            
            Signer = new Signer(this);
            StorageApi = new StorageApi(this);
        }

        public ISigner Signer { get; }
        public IBinarySerializer Serializer => _serializer.Value;
        public IHasher PlainHasher { get; } = new Twox128();
        public IStorageApi StorageApi { get; }

        public int Connect(ConnectionParameters connectionParams, string metadataBlockHash = null)
        {
            if (connectionParams is null)
                throw new ArgumentNullException(nameof(connectionParams));

            _metadataDefaultBlockHash = metadataBlockHash == null
                ? null
                : new GetMetadataParams() {BlockHash = metadataBlockHash};
            int result = Consts.PAPI_OK;

            // Connect to WS
            result = _jsonRpc.Connect(connectionParams);

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
            _protocolParams.BalanceModuleIndex = (byte)_protocolParams.Metadata.GetModuleIndex("Balances", true);
            _protocolParams.TransferMethodIndex = (byte)_protocolParams.Metadata.GetCallMethodIndex("Balances", "transfer");

            _logger.Info($"Balances module index: {_protocolParams.BalanceModuleIndex}");
            _logger.Info($"Transfer call index: {_protocolParams.TransferMethodIndex}");


            try
            {
                _storageKeyBabeGenesisSlot = StorageApi.GetKeys("Babe", "GenesisSlot");
                //_storageKeyBabeCurrentSlot = GetKeys("Babe", "CurrentSlot");
                //_storageKeyBabeEpochIndex = GetKeys("Babe", "EpochIndex");
                _sessionsPerEra = _protocolParams.Metadata.GetConst("Staking", "SessionsPerEra");
                _babeEpochDuration = _protocolParams.Metadata.GetConst("Babe", "EpochDuration");
            }
            catch (ModuleNotFoundException)
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

        internal JObject Request(JObject query, int? timeout = null)
        {
            if (timeout != null)
            {
                return _jsonRpc.Request(query, timeout.Value);
            }
            return _jsonRpc.Request(query);
        }

        public ProtocolParameters GetProtocolParameters()
        {
            return _protocolParams;
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

        public virtual BigInteger GetAccountNonce(Address address)
        {
            // Subscribe to account nonce updates and immediately unsubscribe after response is received
            // This pattern is borrowed from POC-3 UI
            var completionSource = new TaskCompletionSource<BigInteger>();
            var subId = SubscribeAccountInfo(address.Symbols, (accountInfo) =>
            {
                completionSource.SetResult(accountInfo.Nonce);
            });

            UnsubscribeAccountInfo(subId);

            return completionSource.Task.WithTimeout(RequestsTimeout).Sync();
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

        public MetadataBase GetMetadata(GetMetadataParams param, bool force = false)
        {
            param ??= _metadataDefaultBlockHash;
            if (_metadataCache != null && !force)
                return _metadataCache;

            JArray prm = new JArray { };
            if (param != null)
                prm = new JArray { param.BlockHash };
            JObject query = new JObject { { "method", "state_getMetadata" }, { "params", prm } };

            JObject response = _jsonRpc.Request(query);

            try
            {
                var bytes = response["result"].ToString().HexToByteArray();
                var binarySerializer = new BinarySerializer.BinarySerializer(new IndexResolver(), new SerializerSettings());
                var meta = binarySerializer.DeserializeAssertReadAll<RuntimeMetadataPrefixed>(bytes);
                _metadataCache = meta.RuntimeMetadata.IsT12 ? meta.RuntimeMetadata.AsT12 : null;
                return _metadataCache;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual SignedBlock GetBlock(GetBlockParams param)
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

        public NetworkState GetNetworkState()
        {
            JObject query = new JObject { { "method", "system_networkState" },
                                          { "params", new JArray { } } };
            JObject response = _jsonRpc.Request(query);

            if (response == null)
            {
                throw new UnsafeNotAllowedException("system_networkState");
            }

            return Deserialize<NetworkState, ParseNetworkState>(response);
        }

        public PeersInfo GetSystemPeers()
        {
            JObject query = new JObject { { "method", "system_peers" },
                                          { "params", new JArray { } } };
            JObject response = _jsonRpc.Request(query);

            if (response == null)
            {
                throw new UnsafeNotAllowedException("system_peers");
            }

            return Deserialize<PeersInfo, ParsePeersInfo>(response);
        }

        public SystemHealth GetSystemHealth()
        {
            JObject query = new JObject { { "method", "system_health" },
                                          { "params", new JArray { } } };
            JObject response = _jsonRpc.Request(query);

            return Deserialize<SystemHealth, ParseSystemHealth>(response);
        }

        public string SubscribeBlockNumber(Action<long> callback)
        {
            JObject subscribeQuery = new JObject { { "method", "chain_subscribeNewHead" }, { "params", new JArray { } } };

            return Subscribe(subscribeQuery, (json) =>
            {
                var test = json["result"]["number"].ToString().Substring(2);
                var blockNumber = long.Parse(test, NumberStyles.HexNumber);
                callback(blockNumber);
            });
        }

        public string SubscribeRuntimeVersion(Action<RuntimeVersion> callback)
        {
            JObject subscribeQuery = new JObject { { "method", "state_subscribeRuntimeVersion" }, { "params", new JArray { } } };

            return Subscribe(subscribeQuery, (json) =>
            {
                var rtv = Deserialize<RuntimeVersion, ParseRuntimeVersion>(json);
                callback(rtv);
            });
        }

        internal string Subscribe(JObject subscriptionQuery, Action<JObject> parseFunc)
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

        public void UnsubscribeBlockNumber(string id)
        {
            RemoveSubscription(id, "chain_unsubscribeNewHead");
        }

        public void UnsubscribeRuntimeVersion(string id)
        {
            RemoveSubscription(id, "state_unsubscribeRuntimeVersion");
        }

        public void UnsubscribeEraAndSession(string id)
        {
            RemoveSubscription(id, "state_unsubscribeStorage");
        }

        public void UnsubscribeAccountInfo(string id)
        {
            RemoveSubscription(id, "state_unsubscribeStorage");
        }

        public void HandleWsMessage(string subscriptionId, JObject message)
        {
            // subscription already init otherwise subscription does not exist
            Action<JObject> handler = null;

            lock (_subscriptionLock)
            {
                if (!_subscriptionHandlers.TryGetValue(subscriptionId, out handler))
                {
                    _pendingSubscriptionUpdates.TryAdd(subscriptionId, message);
                }
            }

            handler?.Invoke(message);
        }

        internal void RemoveSubscription(string subscriptionId, string method)
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

        public string SignAndSendTransfer(string sender, string privateKey, string recipient, BigInteger amount, Action<string> callback, EraDto era = null, BigInteger? chargeTransactionPayment = null)
        {
            _logger.Info("=== Starting a Transfer Extrinsic ===");
            var from = new Address(sender);
            var privateKeyBytes = privateKey.HexToByteArray();
            var to = new Address(recipient);
            var extrinsic = CreateSignedTransferExtrinsic(from, privateKeyBytes, to, amount, era, chargeTransactionPayment);
            var encodedExtrinsic = CreateSerializer()
                .Serialize(extrinsic)
                .ToPrefixedHexString();

            var query = new JObject { { "method", "author_submitAndWatchExtrinsic" }, { "params", new JArray { encodedExtrinsic } } };

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
                    DataStructs.PublicKey pubk = new DataStructs.PublicKey();
                    pubk.Bytes = genericExtrinsic[count].Signature.SignerPublicKey;
                    genericExtrinsic[count].SignerAddress = _protocolParams.Metadata.GetAddrFromPublicKey(pubk);
                }
                count++;
            }

            return genericExtrinsic.AsSpan()[..count].ToArray();
        }

        internal string ExtrinsicQueryString(byte[] encodedMethodBytes, string module, string method, Address sender, string privateKey)
        {
            _logger.Info("=== Started Invoking Extrinsic ===");
            var publicKey = _protocolParams.Metadata.GetPublicKeyFromAddr(sender);
            var address = new ExtrinsicAddress(publicKey);
            
            var nonce = GetAccountNonce(sender);
            _logger.Info($"sender nonce: {nonce}");
            var extra = new SignedExtra(DefaultEra(), nonce, BigInteger.Zero);
            
            var moduleIndex = (byte)_protocolParams.Metadata.GetModuleIndex(module, true);
            var methodIndex = (byte)_protocolParams.Metadata.GetCallMethodIndex(module, method);
            var call = new ExtrinsicCallRaw<byte[]>(moduleIndex, methodIndex, encodedMethodBytes);
            var extrinsic = new UncheckedExtrinsic<ExtrinsicAddress, ExtrinsicMultiSignature, SignedExtra, ExtrinsicCallRaw<byte[]>>(true, address, null, extra, call);

            Signer.SignUncheckedExtrinsic(extrinsic, publicKey.Bytes, privateKey.HexToByteArray());

            return CreateSerializer()
                .Serialize(AsByteVec.FromValue(extrinsic))
                .ToPrefixedHexString();
        }

        public string SubmitExtrinsic(byte[] encodedMethodBytes, string module, string method, Address sender, string privateKey)
        {
            string teStr = ExtrinsicQueryString(encodedMethodBytes, module, method, sender, privateKey);

            var query = new JObject { { "method", "author_submitExtrinsic" }, { "params", new JArray { teStr } } };
            var response = _jsonRpc.Request(query);

            return response.ToString();
        }

        public string SubmitAndSubcribeExtrinsic(byte[] encodedMethodBytes, string module, string method, Address sender, string privateKey, Action<string> callback)
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

        private byte[] PlainHash<T>(T value)
        {
            using var ms = new MemoryStream();
            Serializer.Serialize(value, ms);
            var bytes = ms.ToArray();
            return PlainHasher.Hash(bytes);
        }

        public string SubscribeEraAndSession(Action<Era, SessionOrEpoch> callback)
        {
            long _bestBlockNum = 0;
            bool done = false;
            var sbnId = SubscribeBlockNumber((blockNum) =>
            {
                _bestBlockNum = blockNum;
                done = true;
            });

            while (!done)
            {
                Thread.SpinWait(1500);
            }
            //UnsubscribeBlockNumber(sbnId);

            // era and session subscription
            JArray _params;
            if (_isEpoch)
            {
                var babeEpochIndex = StorageApi.GetKeys("Babe","EpochIndex");
                var babeGenesisSlot = StorageApi.GetKeys("Babe", "GenesisSlot");
                var babeCurrentSlot = StorageApi.GetKeys("Babe", "CurrentSlot");

                _params =
                    new JArray { new JArray { babeEpochIndex, babeGenesisSlot, babeCurrentSlot } };
            }
            else
            {
                _params =
                    new JArray {
                            new JArray {
                                Consts.LAST_LENGTH_CHANGE_SUBSCRIPTION, Consts.SESSION_LENGTH_SUBSCRIPTION, PlainHash("Staking CurrentEra"),
                                            PlainHash("Staking SessionsPerEra"), PlainHash("Session CurrentIndex")}
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

        public string SubscribeAccountInfo(string address, Action<AccountInfo> callback)
        {
            //string storageKey =
            //           _protocolParams.Metadata.GetAddressStorageKey(_protocolParams.FreeBalanceHasher,
            //           new Address(address), "System Account");

            string key = StorageApi.GetKeys( "System", "Account", new Address(address));

            var subscribeQuery =
                new JObject { { "method", "state_subscribeStorage"}, { "params", new JArray { new JArray { key} } } };


            return Subscribe(subscribeQuery, (json) =>
            {
                var changes = json["result"]["changes"];
                var accountInfoHex = changes[0][1].ToString();
                
                var accountInfo = accountInfoHex.HexToByteArray().ToStruct<AccountInfo>();
                callback(accountInfo);
            });
        }

        internal EraDto DefaultEra()
        {
            //Todo: figure out how to make good mortal era.
            return new EraDto(new ImmortalEra());
            var metadata = GetMetadata(null);
            return FromBlockHashCount(metadata) ?? FromMinimumPeriod(metadata) ?? new EraDto(new ImmortalEra());
        }

        private EraDto FromMinimumPeriod(MetadataBase metadata)
        {
            var timestampModule = metadata.GetModule(KnownModules.Timestamp);
            var minimumPeriodStr = timestampModule
                ?.GetConstant(KnownConstants.MinimumPeriod)
                ?.GetValue();
            if (minimumPeriodStr == null)
            {
                return null;
            }

            var period = CreateSerializer().Deserialize<ulong>(minimumPeriodStr.HexToByteArray());
            var block = GetBlock(null);
            return new EraDto(new MortalEra(period, block.Block.Header.Number));
        }

        private EraDto FromBlockHashCount(MetadataBase metadata)
        {
            var systemModule = metadata.GetModule(KnownModules.System);
            var blockHashCountStr = systemModule
                ?.GetConstant(KnownConstants.BlockHashCount)
                ?.GetValue();

            if (blockHashCountStr == null)
            {
                return null;
            }

            var blockHashCount = CreateSerializer().Deserialize<ulong>(blockHashCountStr.HexToByteArray());

            var period = blockHashCount & (ulong) -(long) blockHashCount;

            var block = GetBlock(null);
            return new EraDto(new MortalEra(period, block.Block.Header.Number));
        }

        internal AsByteVec<UncheckedExtrinsic<ExtrinsicAddress, ExtrinsicMultiSignature, SignedExtra, InheritanceCall<TransferCall>>> CreateSignedTransferExtrinsic(Address from, byte[] privateKeyFrom, Address to, BigInteger amount, EraDto era = null, BigInteger? chargeTransactionPayment = null)
        {
            era ??= DefaultEra();
            chargeTransactionPayment ??= 0;

            var publicKeyFrom = _protocolParams.Metadata.GetPublicKeyFromAddr(from);
            
            var extrinsicAddressFrom = new ExtrinsicAddress(publicKeyFrom);

            var nonce = GetAccountNonce(@from);
            var extra = new SignedExtra(era, nonce, chargeTransactionPayment.Value);

            var publicKeyTo = _protocolParams.Metadata.GetPublicKeyFromAddr(to);
            var call = new TransferCall(publicKeyTo, amount);
            var inheritanceCall = new InheritanceCall<TransferCall>(call);
            var extrinsic = new UncheckedExtrinsic<ExtrinsicAddress, ExtrinsicMultiSignature, SignedExtra, InheritanceCall<TransferCall>>(true, extrinsicAddressFrom, null, extra, inheritanceCall);

            Signer.SignUncheckedExtrinsic(extrinsic, publicKeyFrom.Bytes, privateKeyFrom);

            return AsByteVec.FromValue(extrinsic);
        }

        public IBinarySerializer CreateSerializer()
        {
            var metadata = GetMetadata(null);
            var callModuleIndex = -1;
            var eventModuleIndex = -1;
            var callsLookup = new Dictionary<(string module, string method), (byte module, byte method)?>();
            var eventsLookup = new Dictionary<(string module, string @event), (byte module, byte @event)?>();
            foreach (var module in metadata.GetModules())
            {
                var calls = module.GetCalls();
                if (calls != null && calls.Any())
                {
                    callModuleIndex++;
                    var methodIndex = 0;
                    foreach (var call in calls)
                    {
                        callsLookup[(module.GetName(), call.GetName())] = ((byte) callModuleIndex, (byte) methodIndex++);
                    }
                }

                var events = module.GetEvents();
                if (events != null && events.Any())
                {
                    eventModuleIndex++;
                    var eventIndex = 0;
                    foreach (var @event in events)
                    {
                        eventsLookup[(module.GetName(), @event.GetName())] = ((byte) eventModuleIndex, (byte) eventIndex++);
                    }
                }
            }
            var resolver = new IndexResolver()
            {
                CallIndex = strValue => callsLookup.TryGetOrDefault(strValue, null),
                EventIndex = str => eventsLookup.TryGetOrDefault(str, null),
            };
            return new BinarySerializer.BinarySerializer(resolver, _serializerSettings);
        }

        public static SerializerSettings DefaultSubstrateSettings()
        {
            return new SerializerSettings()
                .AddCall<TransferCall>("Balances", "transfer")
                
                .AddEvent<ExtrinsicSuccess>("System", "ExtrinsicSuccess")
                .AddEvent<ExtrinsicFailed>("System", "ExtrinsicFailed")
                .AddEvent<CodeUpdated>("System", "CodeUpdated")
                .AddEvent<NewAccount>("System", "NewAccount")
                .AddEvent<KilledAccount>("System", "KilledAccount")
                .AddEvent<Transfer>("Contracts", "Transfer")
                .AddEvent<Instantiated>("Contracts", "Instantiated")
                .AddEvent<Evicted>("Contracts", "Evicted")
                .AddEvent<Restored>("Contracts", "Restored")
                .AddEvent<CodeStored>("Contracts", "CodeStored")
                .AddEvent<ScheduleUpdated>("Contracts", "ScheduleUpdated")
                .AddEvent<Dispatched>("Contracts", "Dispatched")
                .AddEvent<ContractExecution>("Contracts", "ContractExecution")
                .AddEvent<NewAuthorities>("Grandpa", "NewAuthorities")
                .AddEvent<Paused>("Grandpa", "Paused")
                .AddEvent<Resumed>("Grandpa", "Resumed")
                .AddEvent<Endowed>("Balances", "Endowed")
                .AddEvent<DustLost>("Balances", "DustLost")
                .AddEvent<Transfer>("Balances", "Transfer")
                .AddEvent<BalanceSet>("Balances", "BalanceSet")
                .AddEvent<Deposit>("Balances", "Deposit")
                .AddEvent<Reserved>("Balances", "Reserved")
                .AddEvent<Unreserved>("Balances", "Unreserved")
                .AddEvent<ReserveRepatriated>("Balances", "ReserveRepatriated")
                .AddEvent<Sudid>("Sudo", "Sudid")
                .AddEvent<KeyChanged>("Sudo", "KeyChanged")
                .AddEvent<SudoAsDone>("Sudo", "SudoAsDone");
        }
    }
}
