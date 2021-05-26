using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.Chain.Rpc;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Chain
{
    public static class ChainModule
    {
        public static IChainRpc<Hash256, SignedBlock<Block<Header<long, Hash256>, byte[]>>, Header<long, Hash256>, long> Chain(this IRpc rpc)
        {
            return rpc.GetModule(() => new ChainRpc<Hash256, SignedBlock<Block<Header<long, Hash256>, byte[]>>, Header<long, Hash256>, long>(rpc));
        }
    }
}