using System;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.Modules.State.Model;
using Polkadot.BinaryContracts;

namespace Polkadot.Api.Client.Modules.State.Rpc
{
    public interface IStateRpc
    {
        Task<RuntimeMetadataPrefixed> GetMetadata(Hash at = null, CancellationToken token = default, TimeSpan? timeout = default);
    }
}