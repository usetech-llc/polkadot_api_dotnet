using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Modules.Chain.Rpc
{
    public interface IChainRpc<THash, TSignedBlock, THeader, TBlockNumber> where TSignedBlock : class
    {
        Task<TSignedBlock> GetBlock(THash hash, CancellationToken token = default);
        Task<THash[]> GetBlockHash(TBlockNumber[] blockNumber, CancellationToken token = default);
        Task<THash> GetBlockHash(TBlockNumber blockNumber = default, CancellationToken token = default);
        Task<THash> GetFinalizedHead(CancellationToken token = default);
        Task<THeader> GetHeader(THash hash = default, CancellationToken token = default);
        Task<ISubscription> SubscribeAllHeads(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default);
        Task<ISubscription> SubscribeFinalizedHeads(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default);
        Task<ISubscription> SubscribeNewHead(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default);
        
    }
}