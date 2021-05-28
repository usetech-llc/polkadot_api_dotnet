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
        /// <summary>
        /// Get header and body of a relay chain block.
        /// </summary>
        Task<TSignedBlock> GetBlock(THash hash, CancellationToken token = default);
        /// <summary>
        /// Get hash of the n-th block in the canon chain.
        ///
        /// By default returns latest block hash.
        /// </summary>
        Task<THash[]> GetBlockHash(TBlockNumber[] blockNumber, CancellationToken token = default);
        /// <summary>
        /// Get hash of the n-th block in the canon chain.
        ///
        /// By default returns latest block hash.
        /// </summary>
        Task<THash> GetBlockHash(TBlockNumber blockNumber = default, CancellationToken token = default);
        /// <summary>
        /// Get hash of the last finalized block in the canon chain.
        /// </summary>
        Task<THash> GetFinalizedHead(CancellationToken token = default);
        /// <summary>
        /// Get header of a relay chain block.
        /// </summary>
        Task<THeader> GetHeader(THash hash = default, CancellationToken token = default);
        /// <summary>
        /// All head subscription
        /// </summary>
        Task<ISubscription> SubscribeAllHeads(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default);
        /// <summary>
        /// Finalized head subscription 
        /// </summary>
        Task<ISubscription> SubscribeFinalizedHeads(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default);
        /// <summary>
        /// New head subscription
        /// </summary>
        Task<ISubscription> SubscribeNewHead(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default);
        
    }
}