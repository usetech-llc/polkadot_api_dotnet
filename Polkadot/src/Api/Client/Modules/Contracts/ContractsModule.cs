using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.Contracts.Rpc;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Modules.Contracts
{
    public static class ContractsModule
    {
        public static IContractsRpc<Hash256, AccountId32, Hash256, long> Contracts(this IRpc rpc)
        {
            return rpc.GetModule(() => new ContractsRpc<Hash256, AccountId32, Hash256, long>(rpc));
        }

        /// <summary>
        /// Executes a call to a contract.
        ///
        /// This call is performed locally without submitting any transactions. Thus executing this
        /// won't change any state. Nonetheless, the calling state-changing contracts is still possible.
        ///
        /// This method is useful for calling getter-like methods on contracts.
        /// </summary>
        public static Task<RpcContractExecResult<TData>> Call<TData, TInput>(
            this IContractsRpc<Hash256, AccountId32, Hash256, long> contractsRpc,
            AccountId32 origin,
            AccountId32 dest,
            UInt128 value,
            UInt256 gasLimit,
            TInput inputData,
            Hash256 at = default,
            CancellationToken token = default)
        {
            var callRequest = CallRequest.Create(origin, dest, value, gasLimit, inputData);
            return contractsRpc.Call<RpcContractExecResult<TData>, CallRequest<AccountId32, UInt128, UInt256, TInput>>(callRequest, at, token);
        }
    }
}