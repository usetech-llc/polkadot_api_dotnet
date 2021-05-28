using System.Threading;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Modules.Contracts.Rpc
{
    public interface IContractsRpc<TBlockHash, TAccountId, THash, TBlockNumber>
    {
        /// <summary>
        /// Executes a call to a contract.
        ///
        /// This call is performed locally without submitting any transactions. Thus executing this
        /// won't change any state. Nonetheless, the calling state-changing contracts is still possible.
        ///
        /// This method is useful for calling getter-like methods on contracts.
        /// </summary>
        Task<TResult> Call<TResult, TCallRequest>(TCallRequest callRequest, TBlockHash at = default, CancellationToken token = default);
        /// <summary>
        /// Returns the value under a specified storage `key` in a contract given by `address` param,
        /// or `None` if it is not set. 
        /// </summary>
        Task<TResult> GetStorage<TResult, TCallRequest>(TAccountId callRequest, THash key, TBlockHash at = default, CancellationToken token = default);

        /// <summary>
        /// Returns the projected time a given contract will be able to sustain paying its rent.
        ///
        /// The returned projection is relevant for the given block, i.e. it is as if the contract was
        /// accessed at the beginning of that block.
        ///
        /// Returns `None` if the contract is exempted from rent.
        /// </summary>
        Task<TBlockNumber> RentProjection(TAccountId address, TBlockHash at = default, CancellationToken token = default);
    }
}