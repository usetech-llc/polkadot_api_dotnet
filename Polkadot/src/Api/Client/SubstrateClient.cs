using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Exceptions;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client
{
    public class SubstrateClient
    {
        public static SubstrateClient<TJsonElement> FromSettings<TJsonElement>(
            SubstrateClientSettings<TJsonElement> settings) where TJsonElement : IJsonElement<TJsonElement>
        {
            return new(settings);
        }
    }
    
    public class SubstrateClient<TJsonElement> : ISubstrateClient<TJsonElement> where TJsonElement : IJsonElement<TJsonElement>
    {
        private SubstrateClientSettings<TJsonElement> Settings { get; }
        private readonly ClientWebSocket _webSocket = new();
        private CancellationTokenSource _listeningCancellation = new();
        private readonly ArrayPool<byte> _buffer = ArrayPool<byte>.Create();
        private long _requestId = 0;
        private int _listenerIsRunning = 0;
        private int _disposed = 0;
        private readonly Lazy<IBinarySerializer> _binarySerializer;
        private readonly Lazy<IJsonSerializer<TJsonElement>> _jsonSerializer;

        public IBinarySerializer BinarySerializer => _binarySerializer.Value;
        public IJsonSerializer<TJsonElement> JsonSerializer => _jsonSerializer.Value;
        public TimeSpan? RpcTimeout => Settings.RpcTimeout;

        public IRpc Rpc { get; }
        public event Action<OneOf<TJsonElement, Exception>> MessageReceived;

        public SubstrateClient(SubstrateClientSettings<TJsonElement> settings)
        {
            Settings = settings;
            Rpc = new Rpc<TJsonElement>(this);
            _binarySerializer = new Lazy<IBinarySerializer>(() => Settings.BinarySerializer(this), LazyThreadSafetyMode.ExecutionAndPublication);
            _jsonSerializer = new Lazy<IJsonSerializer<TJsonElement>>(() => settings.JsonSerializer(this), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public async Task Send(byte[] message, CancellationToken token)
        {
            if (_disposed != 0)
            {
                throw new ObjectDisposedException(nameof(SubstrateClient<TJsonElement>));
            }

            Sending?.Invoke(message);
            await EnsureConnected(token).ConfigureAwait(false);
            await _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, token)
                .ConfigureAwait(false);
        }

        public long NextRequestId()
        {
            return Interlocked.Increment(ref _requestId);
        }

        public event Action<byte[]> Sending;
        public event Action<byte[]> Received;
        public event Action<Exception> Error;

        private async Task Listen()
        {
            List<byte[]> rented = new();
            int lastRentedRead = 0;
            while (true)
            {
                if (_listeningCancellation.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    byte[] rent;
                    if (!rented.Any() || lastRentedRead == rented.Last().Length)
                    {
                        rent = _buffer.Rent(1024 * 64);
                        lastRentedRead = 0;
                        rented.Add(rent);
                    }
                    else
                    {
                        rent = rented.Last();
                    }

                    var arraySegment = new ArraySegment<byte>(rent, lastRentedRead, rent.Length - lastRentedRead);
                    var result = await _webSocket
                        .ReceiveAsync(arraySegment, _listeningCancellation.Token)
                        .ConfigureAwait(false);
                    _listeningCancellation.Token.ThrowIfCancellationRequested();
                    lastRentedRead += result.Count;
                    if (result.EndOfMessage)
                    {
                        var message = new byte[rented.Take(rented.Count - 1).Sum(r => r.Length) + lastRentedRead];
                        var copied = 0;
                        for (int i = 0; i < rented.Count - 1; i++)
                        {
                            Array.Copy(rented[i], 0, message, copied, rented[i].Length);
                            copied += rented[i].Length;
                            _buffer.Return(rented[i]);
                        }
                        Array.Copy(rented.Last(), 0, message, copied, lastRentedRead);
                        _buffer.Return(rented.Last());

                        rented.Clear();
                        Received?.Invoke(message);
                        var element = JsonSerializer.DeserializeToElement(message);
#pragma warning disable 4014
                        Task.Run(() =>
                        {
                            MessageReceived?.Invoke(element);
                            element.Dispose();
                        });
#pragma warning restore 4014
                    }
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex);
                    MessageReceived?.Invoke(new TransportException("Listener failed", ex));
                    if (_webSocket.State != WebSocketState.Open)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10));
                        await Connect(_listeningCancellation.Token);
                    }
                }
            }
        }

        public async Task EnsureConnected(CancellationToken token)
        {
            if (_webSocket.State != WebSocketState.Open)
            {
                await Connect(token);
                if (Interlocked.Exchange(ref _listenerIsRunning, 1) == 0)
                {
#pragma warning disable 4014
                    Task.Run(Listen, token);
                }
#pragma warning restore 4014
            }
        }

        private async Task Connect(CancellationToken token)
        {
            try
            {
                await _webSocket.ConnectAsync(new Uri(Settings.RpcEndpoint), token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageReceived?.Invoke(new TransportException("Failed to connect", ex));
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                return;
            }
            _listeningCancellation.Cancel();
            _webSocket?.Dispose();
            _listeningCancellation?.Dispose();
        }
    }
}