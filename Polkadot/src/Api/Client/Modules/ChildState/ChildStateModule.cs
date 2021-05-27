using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.ChildState.Rpc;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Сhildstate
{
    public static class ChildStateModule
    {
        public static IChildStateRpc<Hash256> ChildState(this IRpc rpc)
        {
            return rpc.GetModule(() => new ChildStateRpc<Hash256>(rpc));
        }
    }
}