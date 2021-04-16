using Polkadot.Api.Client.Modules.State.Rpc;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.State
{
    public static class StateModule
    {
        public static IStateRpc State(this IRpc rpc)
        {
            return rpc.GetModule(() => new StateRpc(rpc));
        }
    }
}