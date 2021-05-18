using System;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Modules.Model;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Modules.Author.Rpc
{
    public interface IAuthorRpc
    {
        Task<bool> HasKey(byte[] publicKey, string keyType, CancellationToken token = default);
        Task<bool> HasSessionKeys(byte[] sessionKeys, CancellationToken token = default);
        Task<Unit> InsertKey(string keyType, string suri, byte[] @public, CancellationToken token = default);
        Task<byte[][]> PendingExtrinsics(CancellationToken token = default);
        Task<THash[]> RemoveExtrinsic<THash>(ExtrinsicOrHash<THash>[] hashes, CancellationToken token = default);
        Task<byte[]> RotateKeys(CancellationToken token = default);
    }
}