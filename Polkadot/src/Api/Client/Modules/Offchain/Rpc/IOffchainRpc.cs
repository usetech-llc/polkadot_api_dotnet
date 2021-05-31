using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Modules.Offchain.Model;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Modules.Offchain.Rpc
{
    public interface IOffchainRpc
    {
        /// <summary>
        /// Get offchain local storage under given key and prefix. 
        /// </summary>
        Task<TStorageItem> LocalStorageGet<TStorageItem, TKey>(StorageKind kind, TKey key, CancellationToken token = default);

        /// <summary>
        /// Set offchain local storage under given key and prefix.
        /// </summary>
        Task<Unit> LocalStorageSet<TStorageItem, TKey>(StorageKind kind, TKey key, TStorageItem value, CancellationToken token = default);
    }
}