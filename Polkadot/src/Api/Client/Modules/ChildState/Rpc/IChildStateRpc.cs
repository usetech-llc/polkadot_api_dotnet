using System.Threading;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Modules.ChildState.Rpc
{
    public interface IChildStateRpc<THash>
    {
        /// <summary>
        /// Returns the keys with prefix from a child storage, leave empty to get all the keys 
        /// </summary>
        Task<TResultKey[]> GetKeys<TStorageKey, TPrefixedStorageKey, TResultKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
        /// <summary>
        /// Returns a child storage entry at a specific block's state. 
        /// </summary>
        Task<TStorageData> GetStorage<TStorageKey, TPrefixedStorageKey, TStorageData>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
        /// <summary>
        /// Returns the hash of a child storage entry at a block's state. 
        /// </summary>
        Task<THash> GetStorageHash<TStorageKey, TPrefixedStorageKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
        /// <summary>
        /// Returns the size of a child storage entry at a block's state. 
        /// </summary>
        Task<ulong> GetStorageSize<TStorageKey, TPrefixedStorageKey>(TPrefixedStorageKey childStorageKey, TStorageKey prefix, THash hash = default, CancellationToken token = default);
    }
}