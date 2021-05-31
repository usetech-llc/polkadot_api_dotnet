using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.Payment.Rpc;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Payment
{
    public static class PaymentModule
    {
        public static IPaymentRpc<Hash256> Payment(this IRpc rpc)
        {
            return rpc.GetModule(() => new PaymentRpc<Hash256>(rpc));
        }
    }
}