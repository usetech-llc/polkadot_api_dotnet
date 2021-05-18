using System;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Modules.Model;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Modules.Author.Rpc
{
    public class AuthorRpc : IAuthorRpc
    {
        private readonly IRpc _rpc;

        public AuthorRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<bool> HasKey(byte[] publicKey, string keyType, CancellationToken token = default)
        {
            return _rpc.Call<bool>("author_hasKey", new object[] {publicKey, keyType}, token);
        }

        public Task<bool> HasSessionKeys(byte[] sessionKeys, CancellationToken token = default)
        {
            return _rpc.Call<bool>("author_hasSessionKeys", new[] {sessionKeys}, token);
        }

        public Task<Unit> InsertKey(string keyType, string suri, byte[] @public, CancellationToken token = default)
        {
            return _rpc.Call<Unit>("author_insertKey", new object[] {keyType, suri, @public}, token);
        }

        public Task<byte[][]> PendingExtrinsics(CancellationToken token = default)
        {
            return _rpc.Call<byte[][]>("author_pendingExtrinsics", null, token);
        }

        public Task<THash[]> RemoveExtrinsic<THash>(ExtrinsicOrHash<THash>[] hashes, CancellationToken token = default)
        {
            return _rpc.Call<THash[]>("author_removeExtrinsic", new[] {hashes}, token);
        }

        public Task<byte[]> RotateKeys(CancellationToken token = default)
        {
            return _rpc.Call<byte[]>("author_rotateKeys", null, token);
        }
    }
}