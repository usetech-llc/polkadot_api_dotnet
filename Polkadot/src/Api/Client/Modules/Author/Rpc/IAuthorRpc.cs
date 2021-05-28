using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.Model;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinaryContracts.Extrinsic;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Modules.Author.Rpc
{
    public interface IAuthorRpc<TExtrinsicOrHash, THash, TTransactionStatus>
    {
        /// <summary>
        /// Checks if the keystore has private keys for the given public key and key type.
        ///
        /// Returns `true` if a private key could be found.
        /// </summary>
        Task<bool> HasKey(byte[] publicKey, string keyType, CancellationToken token = default);
        /// <summary>
        /// Checks if the keystore has private keys for the given session public keys.
        ///
        /// `session_keys` is the SCALE encoded session keys object from the runtime.
        ///
        /// Returns `true` iff all private keys could be found.
        /// </summary>
        Task<bool> HasSessionKeys(byte[] sessionKeys, CancellationToken token = default);
        /// <summary>
        /// Insert a key into the keystore.
        /// </summary>
        Task<Unit> InsertKey(string keyType, string suri, byte[] @public, CancellationToken token = default);
        /// <summary>
        /// Returns all pending extrinsics, potentially grouped by sender.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<byte[][]> PendingExtrinsics(CancellationToken token = default);
        /// <summary>
        /// Remove given extrinsic from the pool and temporarily ban it to prevent reimporting.
        /// </summary>
        Task<THash[]> RemoveExtrinsic(TExtrinsicOrHash[] hashes, CancellationToken token = default);
        /// <summary>
        /// Generate new session keys and returns the corresponding public keys. 
        /// </summary>
        Task<byte[]> RotateKeys(CancellationToken token = default);
        /// <summary>
        /// Submit an extrinsic to watch.
        ///
        /// See [`TransactionStatus`](sp_transaction_pool::TransactionStatus) for details on transaction
        /// life cycle.
        /// </summary>
        Task<ISubscription> SubmitAndWatchExtrinsic<TExtrinsic>(
            Func<OneOf<TTransactionStatus, Exception>, Task> onMessage, 
            TExtrinsic extrinsic, 
            bool keepAlive = false, 
            CancellationToken token = default
        );
        /// <summary>
        /// Submit hex-encoded extrinsic for inclusion in block. 
        /// </summary>
        Task<THash> SubmitExtrinsic<TExtrinsic>(TExtrinsic extrinsic, CancellationToken token = default);
    }
}