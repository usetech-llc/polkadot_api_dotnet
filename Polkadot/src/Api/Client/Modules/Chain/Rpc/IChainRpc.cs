using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Api.Client.Modules.Chain.Rpc
{
    public interface IChainRpc
    {
        Task<ISubscription> SubscribeNewHead(Func<OneOf<Header<ulong, Hash256>, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default);
    }
}