using Polkadot.Api.Client.Modules.Offchain.Rpc;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Offchain
{
    public static class OffchainModule
    {
        public static IOffchainRpc Offchain(this IRpc rpc)
        {
            return rpc.GetModule(() => new OffchainRpc(rpc));
        }
    }
}