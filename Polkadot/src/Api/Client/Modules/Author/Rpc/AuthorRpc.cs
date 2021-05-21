using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Modules.Author.Rpc
{
    public class AuthorRpc<TExtrinsicOrHash, THash, TTransactionStatus> : IAuthorRpc<TExtrinsicOrHash, THash, TTransactionStatus>
    {
        private readonly IRpc _rpc;

        public AuthorRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<bool> HasKey(byte[] publicKey, string keyType, CancellationToken token = default)
        {
            return _rpc.Call<bool>("author_hasKey", token, publicKey, keyType);
        }

        public Task<bool> HasSessionKeys(byte[] sessionKeys, CancellationToken token = default)
        {
            return _rpc.Call<bool>("author_hasSessionKeys", token, sessionKeys);
        }

        public Task<Unit> InsertKey(string keyType, string suri, byte[] @public, CancellationToken token = default)
        {
            return _rpc.Call<Unit>("author_insertKey", token, keyType, suri, @public);
        }

        public Task<byte[][]> PendingExtrinsics(CancellationToken token = default)
        {
            return _rpc.Call<byte[][]>("author_pendingExtrinsics", token);
        }

        public Task<THash[]> RemoveExtrinsic(TExtrinsicOrHash[] hashes, CancellationToken token = default)
        {
            return _rpc.Call<THash[]>("author_removeExtrinsic", token, new object[] {hashes});
        }

        public Task<byte[]> RotateKeys(CancellationToken token = default)
        {
            return _rpc.Call<byte[]>("author_rotateKeys", token);
        }

        public Task<ISubscription> SubmitAndWatchExtrinsic<TExtrinsic>(Func<OneOf<TTransactionStatus, Exception>, Task> onMessage, TExtrinsic extrinsic, bool keepAlive = false,
            CancellationToken token = default)
        {
            return _rpc.Subscribe(
                "author_extrinsicUpdate",
                "author_submitAndWatchExtrinsic",
                "author_unwatchExtrinsic",
                onMessage,
                keepAlive,
                token, extrinsic);
        }

        public Task<THash> SubmitExtrinsic<TExtrinsic>(TExtrinsic extrinsic, CancellationToken token = default)
        {
            return _rpc.Call<THash>("author_submitExtrinsic", token, extrinsic);
        }
    }
}