using System;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.BinaryContracts;
using Polkadot.DataStructs.Metadata.BinaryContracts;

namespace Polkadot.Api.Client.Modules.State.Rpc
{
    public interface IStateRpc
    {
        Task<RuntimeMetadataPrefixed> GetMetadata(Hash at = null, CancellationToken token = default, TimeSpan? timeout = default);
    }
}