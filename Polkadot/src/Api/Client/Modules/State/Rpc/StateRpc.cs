using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.State.Model;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinaryContracts;

namespace Polkadot.Api.Client.Modules.State.Rpc
{
    public class StateRpc : IStateRpc
    {
        private readonly IRpc _rpc;

        public StateRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<RuntimeMetadataPrefixed> GetMetadata<THash>(THash at = default, CancellationToken token = default)
        {
            if (EqualityComparer<THash>.Default.Equals(at, default))
            {
                return _rpc.Call<RuntimeMetadataPrefixed>("state_getMetadata", token);
            }

            return _rpc.Call<RuntimeMetadataPrefixed>("state_getMetadata", token, at);
        }
    }
}