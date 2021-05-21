using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.Author.Rpc;
using Polkadot.Api.Client.Modules.Model;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Author
{
    public static class AuthorModule
    {
        public static IAuthorRpc<ExtrinsicOrHash<THash>, THash, TransactionStatus<THash, THashBlock>> Author<THash, THashBlock>(this IRpc rpc)
        {
            return rpc.GetModule(() => new AuthorRpc<ExtrinsicOrHash<THash>, THash, TransactionStatus<THash, THashBlock>>(rpc));
        }
        public static IAuthorRpc<ExtrinsicOrHash<Hash256>, Hash256, TransactionStatus<Hash256, Hash256>> Author(this IRpc rpc)
        {
            return rpc.Author<Hash256, Hash256>();
        }
    }
}