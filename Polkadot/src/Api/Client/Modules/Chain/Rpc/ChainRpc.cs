using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer.Types;
using Polkadot.Extensions;

namespace Polkadot.Api.Client.Modules.Chain.Rpc
{
    public class ChainRpc<THash, TSignedBlock, THeader, TBlockNumber> : IChainRpc<THash, TSignedBlock, THeader, TBlockNumber> where TSignedBlock: class
    {
        private readonly IRpc _rpc;

        public ChainRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<TSignedBlock> GetBlock(THash hash, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<TSignedBlock, THash>("chain_getBlock", token, hash);
        }

        public Task<THash[]> GetBlockHash(TBlockNumber[] blockNumber, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<THash[], TBlockNumber[]>("chain_getBlockHash", token, blockNumber);
        }

        public Task<THash> GetBlockHash(TBlockNumber blockNumber = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<THash, TBlockNumber?>("chain_getBlockHash", token, blockNumber);
        }

        public Task<THash> GetFinalizedHead(CancellationToken token = default)
        {
            return _rpc.Call<THash>("chain_getFinalizedHead", token);
        }

        public Task<THeader> GetHeader(THash hash = default, CancellationToken token = default)
        {
            return _rpc.CallWithOptionalParam<THeader, THash>("chain_getHeader", token, hash);
        }

        public Task<ISubscription> SubscribeAllHeads(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default)
        {
            return _rpc.Subscribe("chain_allHead", "chain_subscribeAllHeads", "chain_unsubscribeAllHeads", onMessage,
                keepAlive, token);
        }

        public Task<ISubscription> SubscribeFinalizedHeads(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default)
        {
            return _rpc.Subscribe("chain_finalizedHead", "chain_subscribeFinalizedHeads",
                "chain_unsubscribeFinalizedHeads", onMessage, keepAlive, token);
        }

        public Task<ISubscription> SubscribeNewHead(Func<OneOf<THeader, Exception>, Task> onMessage, bool keepAlive = false, CancellationToken token = default)
        {
            return _rpc.Subscribe("chain_newHead", "chain_subscribeNewHead", "chain_unsubscribeNewHead", onMessage,
                keepAlive, token);
        }
    }
}