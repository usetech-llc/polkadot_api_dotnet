using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Extensions;

namespace Polkadot.Api.Client.Modules.Contracts.Rpc
{
    public class ContractsRpc<TBlockHash, TAccountId, THash, TBlockNumber> : IContractsRpc<TBlockHash, TAccountId, THash, TBlockNumber>
    {
        private readonly IRpc _rpc;

        public ContractsRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<TResult> Call<TResult, TCallRequest>(TCallRequest callRequest, TBlockHash at = default,
            CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<TResult, TCallRequest, TBlockHash>("contracts_call", token, callRequest,
                at);
        }

        public Task<TResult> GetStorage<TResult, TCallRequest>(TAccountId callRequest, THash key, TBlockHash at = default,
            CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<TResult, TAccountId, THash, TBlockHash>("contracts_getStorage", token,
                callRequest, key, at);
        }

        public Task<TBlockNumber> RentProjection(TAccountId address, TBlockHash at = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<TBlockNumber, TAccountId, TBlockHash>("contracts_rentProjection", token,
                address, at);
        }
    }
}