using System;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Model;
using Polkadot.Api.Client.Modules.State.Model;
using Polkadot.BinaryContracts;

namespace Polkadot.Api.Client.Modules.State.Rpc
{
    public interface IStateRpc
    {
        Task<RuntimeMetadataPrefixed> GetMetadata<THash>(THash at = default, CancellationToken token = default);
    }
}