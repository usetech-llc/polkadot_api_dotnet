using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Chain.Rpc
{
    public class ChainRpc : IChainRpc
    {
        private readonly IRpc _rpc;

        public ChainRpc(IRpc rpc)
        {
            _rpc = rpc;
        }
        
        public Task<ISubscription> SubscribeNewHead(Func<OneOf<Header<ulong, Hash256>, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default)
        {
            return _rpc.Subscribe("chain_newHead", "chain_subscribeNewHead", "chain_unsubscribeNewHead", onMessage,
                keepAlive, token);
        }
    }
}