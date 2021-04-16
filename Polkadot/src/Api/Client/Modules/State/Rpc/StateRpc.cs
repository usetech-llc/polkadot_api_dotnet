using System;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinaryContracts;
using Polkadot.DataStructs.Metadata.BinaryContracts;

namespace Polkadot.Api.Client.Modules.State.Rpc
{
    public class StateRpc : IStateRpc
    {
        private readonly IRpc _rpc;

        public StateRpc(IRpc rpc)
        {
            _rpc = rpc;
        }

        public Task<RuntimeMetadataPrefixed> GetMetadata(Hash at = null, CancellationToken token = default, TimeSpan? timeout = default)
        {
            var p = at == null ? null : new[] {at};
            return _rpc.Call<RuntimeMetadataPrefixed>("state_getMetadata", p, token, timeout);
        }
    }
}