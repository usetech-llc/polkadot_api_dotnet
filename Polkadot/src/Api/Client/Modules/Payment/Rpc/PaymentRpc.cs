using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Extensions;

namespace Polkadot.Api.Client.Modules.Payment.Rpc
{
    public class PaymentRpc<TBlockHash> : IPaymentRpc<TBlockHash>
    {
        private readonly IRpc _rpc;

        public PaymentRpc(IRpc rpc)
        {
            _rpc = rpc;
        }
        
        public Task<TResponse> QueryInfo<TResponse, TEncodedXt>(TEncodedXt encodedXt, TBlockHash at = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<TResponse, TEncodedXt, TBlockHash>("payment_queryInfo", token, encodedXt, at);
        }
    }
}