using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Extensions;

namespace Polkadot.Api.Client.Modules.ChildState.Rpc
{
    public class ChildStateRpc<THash> : IChildStateRpc<THash>
    {
        private readonly IRpc _rpc;

        public ChildStateRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<TResultKey[]> GetKeys<TStorageKey, TPrefixedStorageKey, TResultKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix,
            THash hash = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<TResultKey[], TPrefixedStorageKey, TStorageKey, THash>("childstate_getKeys", token, childStorageKey, prefix, hash);
        }

        public Task<TStorageData> GetStorage<TStorageKey, TPrefixedStorageKey, TStorageData>(TPrefixedStorageKey childStorageKey, TStorageKey prefix,
            THash hash = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<TStorageData, TPrefixedStorageKey, TStorageKey, THash>("childstate_getStorage", token, childStorageKey, prefix, hash);
        }

        public Task<THash> GetStorageHash<TStorageKey, TPrefixedStorageKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix,
            THash hash = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<THash, TPrefixedStorageKey, TStorageKey, THash>("childstate_getStorageHash", token, childStorageKey, prefix, hash);
        }

        public Task<ulong> GetStorageSize<TStorageKey, TPrefixedStorageKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix,
            THash hash = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<ulong, TPrefixedStorageKey, TStorageKey, THash>("childstate_getStorageSize", token, childStorageKey, prefix, hash);
        }
    }
}