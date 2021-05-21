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
        Task<bool> HasKey(byte[] publicKey, string keyType, CancellationToken token = default);
        Task<bool> HasSessionKeys(byte[] sessionKeys, CancellationToken token = default);
        Task<Unit> InsertKey(string keyType, string suri, byte[] @public, CancellationToken token = default);
        Task<byte[][]> PendingExtrinsics(CancellationToken token = default);
        Task<THash[]> RemoveExtrinsic(TExtrinsicOrHash[] hashes, CancellationToken token = default);
        Task<byte[]> RotateKeys(CancellationToken token = default);
        Task<ISubscription> SubmitAndWatchExtrinsic<TExtrinsic>(
            Func<OneOf<TTransactionStatus, Exception>, Task> onMessage, 
            TExtrinsic extrinsic, 
            bool keepAlive = false, 
            CancellationToken token = default
        );

        Task<THash> SubmitExtrinsic<TExtrinsic>(TExtrinsic extrinsic, CancellationToken token = default);
    }
}