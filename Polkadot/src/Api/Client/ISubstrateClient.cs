using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client
{
    public interface ISubstrateClient : IDisposable
    {
        IBinarySerializer BinarySerializer { get; }
        IRpc Rpc { get; }
        
        event Action<OneOf<JsonDocument, Exception>> OnMessageReceived;
        Task Send(byte[] message, CancellationToken token);
        long NextRequestId();
    }
}