using Newtonsoft.Json.Linq;
using Polkadot.Api.Hashers;
using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;

namespace Polkadot.Api
{
    using System;
    using System.Numerics;
    using Polkadot.Data;
    using Polkadot.DataStructs;
    using Polkadot.DataStructs.Metadata;
    using Polkadot.src.DataStructs;

    public interface IApplication : IDisposable
    {
        ISigner Signer { get; }
        IBinarySerializer Serializer { get; }
        IHasher PlainHasher { get; }
        IStorageApi StorageApi { get; }

        int Connect(ConnectionParameters connectionParams, string metadataBlockHash = null);

        void Disconnect();

        ProtocolParameters GetProtocolParameters();

        /// <summary>
        /// Retreives the current nonce for specific address
        /// </summary>
        /// <param name="address"> the address to get nonce for </param>
        /// <returns> address nonce </returns>
        BigInteger GetAccountNonce(Address address);

        /// <summary>
        /// Call 4 methods and put them together in a single object
        /// system_chain
        /// system_name
        /// system_version
        /// system_properties
        /// </summary>
        SystemInfo GetSystemInfo();

        /// <summary>
        ///  Retreives the block hash for specific block
        /// </summary>
        /// <param name=""> struct with blockNumber block number </param>
        /// <returns> BlockHash struct with result </returns>
        BlockHash GetBlockHash(GetBlockHashParams param);

        /// <summary>
        ///  Retreives the runtime version information for specific block
        /// </summary>
        /// <param name=""> struct with blockHash 64 diget number in hex format </param>
        /// <returns> RuntimeVersion struct with result </returns>            
        RuntimeVersion GetRuntimeVersion(GetRuntimeVersionParams param);

        /// <summary>
        ///  Get header and body of a relay chain block
        /// </summary>
        /// <param name=""> struct with blockHash 64 diget number in hex format </param>
        /// <returns> SignedBlock struct with result </returns>     
        SignedBlock GetBlock(GetBlockParams param);

        /// <summary>
        /// Retrieves the header for a specific block
        /// </summary>
        /// <param name="param"> struct with blockHash 64 diget number in hex format </param>
        /// <returns> BlockHeader struct with result </returns>
        BlockHeader GetBlockHeader(GetBlockParams param);

        /// <summary>
        /// Returns current state of the network
        /// </summary>
        /// <returns>
        /// NetworkState struct with result
        /// </returns>
        NetworkState GetNetworkState();

        /// <summary>
        /// Get hash of the last finalized block in the chain
        /// </summary>
        /// <returns> FinalHead struct with result </returns>
        FinalHead GetFinalizedHead();

        /// <summary>
        ///  Retreives the runtime metadata for specific block
        /// </summary>
        /// <param name="param"> (optional) struct with blockHash 64 diget number in hex format </param>
        /// <param name="force"> (default false) use cache </param>
        /// <returns> Metadata struct with result </returns>
        MetadataBase GetMetadata(GetMetadataParams param, bool force = false);

        /// <summary>
        /// Returns the currently connected peers
        /// </summary>
        /// <returns>
        /// PeersInfo struct with result
        /// </returns>
        PeersInfo GetSystemPeers();

        /// <summary>
        /// Return health status of the node
        /// </summary>
        /// <returns>
        /// SystemHealth struct with result
        /// </returns>
        SystemHealth GetSystemHealth();

        /// <summary>
        /// Sign a transfer with provided private key, submit it to blockchain, and wait for completion. Once transaction is
        /// accepted, the callback will be called with parameter "ready". Once completed, the callback will be called with
        /// completion result string equal to "finalized".
        /// </summary>
        /// <param name="sender"> address of sender (who signs the transaction) </param>
        /// <param name="privateKey"> 64 byte private key of signer in hex, 2 symbols per byte (e.g. "0102ABCD...") </param>
        /// <param name="recipient"> address that will receive the transfer </param>
        /// <param name="amount"> amount (in femto DOTs) to transfer </param>
        /// <param name="callback"> delegate that will receive operation updates </param>
        /// <param name="era">Valid era for transaction to be included into blockchain. Node settings will be used by default.</param>
        /// <param name="chargeTransactionPayment">Tips. Default is 0.</param>
        string SignAndSendTransfer(string sender, string privateKey, string recipient, BigInteger amount, Action<string> callback, EraDto era = null, BigInteger? chargeTransactionPayment = null);

        /// <summary>
        /// Subscribe to era and session. Only one subscription at a time is allowed. If a subscription already
        ///  exists, old subscription will be discarded and replaced with the new one.Until subscribeEraAndSession method is
        ///  called, the API will be receiving updates and forwarding them to subscribed object/function.Only
        ///  unsubscribeBlockNumber will physically unsubscribe from WebSocket endpoint updates.
        /// </summary>
        /// <param name="callback"> expression that will receive updates </param>
        /// <returns> operation result </returns>
        string SubscribeEraAndSession(Action<Era, SessionOrEpoch> callback);

        /// <summary>
        /// Subscribe to most recent account info for a given address. Only one subscription at a time per address is allowed. If
        ///     a subscription already exists for the same address, old subscription will be discarded and replaced with the new
        ///     one. Until <see cref="UnsubscribeAccountInfo"/> method is called with the same address, the API will be receiving updates and
        ///     forwarding them to subscribed object/function.Only unsubscribeBalance will physically unsubscribe from WebSocket
        ///     endpoint updates.
        /// </summary>
        /// <param name="address"> address to receive updates for </param>
        /// <param name="callback">  expression that will receive account info updates </param>
        /// <returns> Subscription id </returns>
        string SubscribeAccountInfo(string address, Action<AccountInfo> callback);

        /// <summary>
        /// Returns all pending extrinsics
        /// </summary>
        /// <param name="bufferSize"> size of preallocated array </param>
        /// <returns> 
        ///     Extrinsics received from the node (may be greater than buffer size, in which case items with
        ///     indexes greater than bufferSize are not returned) 
        /// </returns>
        GenericExtrinsic[] PendingExtrinsics(int bufferSize);

        /// <summary>
        /// Submit a fully formatted extrinsic for block inclusion
        /// </summary>
        /// <param name="encodedMethodBytes"> encoded extrintic parametrs </param>
        /// <param name="module"> invoked module name </param>
        /// <param name="method"> invoked method name </param>
        /// <param name="sender"> sender address </param>
        /// <param name="privateKey"> sender private key </param>
        /// <returns> Extrinsic hash </returns>
        string SubmitExtrinsic(byte[] encodedMethodBytes, string module, string method, Address sender, string privateKey);

        /// <summary>
        /// Remove given extrinsic from the pool and temporarily ban it to prevent reimporting
        /// </summary>
        /// <param name="extrinsicHash"> hash of extrinsic as returned by submitExtrisic </param>
        /// <returns> Operation result </returns>
        bool RemoveExtrinsic(string extrinsicHash);

        /// <summary>
        /// Subscribe to most recent block number.Only one subscription at a time is allowed.If a subscription already 
        /// exists, old subscription will be discarded and replaced with the new one.Until unsubscribeBlockNumber method is
        /// called, the API will be receiving updates and forwarding them to subscribed object/function.Only
        /// unsubscribeBlockNumber will physically unsubscribe from WebSocket endpoint updates.
        /// </summary>
        /// <param name="callback"> callback - delegate that will receive updates</param>
        /// <returns> operation result </returns>
        string SubscribeBlockNumber(Action<long> callback);

        /// <summary>
        /// Unsubscribe from WebSocket endpoint and stop receiving updates with most recent block number.
        /// </summary>
        /// <param name="id"> Subscription id </param>
        /// <returns> operation result </returns>
        void UnsubscribeBlockNumber(string id);

        /// <summary>
        /// Subscribe to most recent runtime version.This subscription is necessary for applications that keep connection
        /// for a long time.If update about runtime version arrives, it will be necessary to disconnect and reconnect since
        /// module and method indexes might have changed.
        /// 
        /// Only one subscription at a time is allowed.If a subscription already
        /// exists, old subscription will be discarded and replaced with the new one.Until unsubscribeRuntimeVersion method
        /// is called, the API will be receiving updates and forwarding them to subscribed object/function.Only
        /// unsubscribeRuntimeVersion will physically unsubscribe from WebSocket endpoint updates.
        /// </summary>
        /// <param name="callback"> callback - delegate that will receive updates </param>
        /// <returns> operation result </returns>
        string SubscribeRuntimeVersion(Action<RuntimeVersion> callback);

        /// <summary>
        /// Submit and subscribe a fully formatted extrinsic for block inclusion
        /// </summary>
        /// <param name="encodedMethodBytes"> encoded extrintic parametrs </param>
        /// <param name="module"> invokable module name </param>
        /// <param name="method"> invokable method name</param>
        /// <param name="sender"> sender address </param>
        /// <param name="privateKey">  sender private key </param>
        /// <param name="callback"> expression that will receive operation updates </param>
        /// <returns></returns>
        string SubmitAndSubcribeExtrinsic(byte[] encodedMethodBytes,
                                        string module, string method, Address sender, string privateKey,
                                       Action<string> callback);

        /// <summary>
        /// Unsubscribe from WebSocket endpoint and stop receiving updates with most recent Runtime Version.
        /// </summary>
        /// <param name="id"> Subscription id </param>
        void UnsubscribeRuntimeVersion(string id);

        /// <summary>
        /// Unsubscribe from WebSocket endpoint and stop receiving updates with era and session.
        /// </summary>
        /// <param name="id"> Subscription id </param>
        void UnsubscribeEraAndSession(string id);

        /// <summary>
        /// Unsubscribe from WebSocket endpoint and stop receiving updates for address account info.
        /// </summary>
        /// <param name="id"> Subscription id </param>
        void UnsubscribeAccountInfo(string id);

        string SignAndSendExtrinsic<TCall>(Address from, byte[] privateKeyFrom, TCall call, Action<string> callback,
            EraDto era = null, BigInteger? chargeTransactionPayment = null) where TCall : IExtrinsicCall;

    }
}
