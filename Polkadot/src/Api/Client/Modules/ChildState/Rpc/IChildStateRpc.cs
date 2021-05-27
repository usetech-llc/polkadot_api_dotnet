using System.Threading;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Modules.ChildState.Rpc
{
    public interface IChildStateRpc<THash>
    {
        Task<TResultKey[]> GetKeys<TStorageKey, TPrefixedStorageKey, TResultKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
        Task<TStorageData> GetStorage<TStorageKey, TPrefixedStorageKey, TStorageData>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
        Task<THash> GetStorageHash<TStorageKey, TPrefixedStorageKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
        Task<ulong> GetStorageSize<TStorageKey, TPrefixedStorageKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
    }
}