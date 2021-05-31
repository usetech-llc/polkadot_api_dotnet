using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Modules.Offchain.Model;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer.Types;
using Polkadot.Extensions;

namespace Polkadot.Api.Client.Modules.Offchain.Rpc
{
    public class OffchainRpc : IOffchainRpc
    {
        private readonly IRpc _rpc;

        public OffchainRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<TStorageItem> LocalStorageGet<TStorageItem, TKey>(StorageKind kind, TKey key, CancellationToken token = default)
        {
            return _rpc.Call<TStorageItem>("offchain_localStorageGet", token, kind, key);
        }

        public Task<Unit> LocalStorageSet<TStorageItem, TKey>(StorageKind kind, TKey key, TStorageItem value,
            CancellationToken token = default)
        {
            return _rpc.Call<Unit>("offchain_localStorageSet", token, kind, key, value);
        }
    }
}