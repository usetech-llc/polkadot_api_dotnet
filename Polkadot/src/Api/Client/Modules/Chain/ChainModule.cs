using Polkadot.Api.Client.Modules.Chain.Rpc;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Chain
{
    public static class ChainModule
    {
        public static IChainRpc Chain(this IRpc rpc)
        {
            return rpc.GetModule(() => new ChainRpc(rpc));
        }

    }
}