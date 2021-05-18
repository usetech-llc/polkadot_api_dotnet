using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client
{
    public interface ISubstrateClient<TJsonElement> : IDisposable where TJsonElement : IJsonElement<TJsonElement>
    {
        IBinarySerializer BinarySerializer { get; }
        IJsonSerializer<TJsonElement> JsonSerializer { get; }
        TimeSpan? RpcTimeout { get; }
        IRpc Rpc { get; }
        
        event Action<OneOf<TJsonElement, Exception>> MessageReceived;
        Task Send(byte[] message, CancellationToken token);
        long NextRequestId();

        event Action<byte[]> Sending;
        event Action<byte[]> Received;
        event Action<Exception> Error;
    }
}