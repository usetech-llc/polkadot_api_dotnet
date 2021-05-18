using Polkadot.Api.Client.Modules.Author.Rpc;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Author
{
    public static class AuthorModule
    {
        public static IAuthorRpc Author(this IRpc rpc)
        {
            return rpc.GetModule(() => new AuthorRpc(rpc));
        }
    }
}