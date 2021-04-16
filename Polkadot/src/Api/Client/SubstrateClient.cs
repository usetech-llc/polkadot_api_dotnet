using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Exceptions;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client
{
    public class SubstrateClient : ISubstrateClient
    {
        private SubstrateClientSettings Settings { get; }
        private readonly ClientWebSocket _webSocket = new();
        private CancellationTokenSource _listeningCancellation = new();
        private readonly ArrayPool<byte> _buffer = ArrayPool<byte>.Create();
        private long _requestId = 0;
        private int _listenerIsRunning = 0;

        public IBinarySerializer BinarySerializer => Settings.BinarySerializer;
        public IRpc Rpc { get; }
        public event Action<OneOf<JsonDocument, Exception>> OnMessageReceived;

        public SubstrateClient(SubstrateClientSettings settings)
        {
            Settings = settings;
            Rpc = new Rpc(this);
        }

        public async Task Send(byte[] message, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await EnsureConnected(token).ConfigureAwait(false);
            await _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, token)
                .ConfigureAwait(false);
        }

        public long NextRequestId()
        {
            return Interlocked.Increment(ref _requestId);
        }

        private async Task Listen()
        {
            List<byte[]> rented = new();
            List<int> chunkLengths = new();
            while (true)
            {
                if (_listeningCancellation.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    var rent = _buffer.Rent(1024 * 64);
                    rented.Add(rent);
                    var result = await _webSocket
                        .ReceiveAsync(new ArraySegment<byte>(rent), _listeningCancellation.Token).ConfigureAwait(false);
                    _listeningCancellation.Token.ThrowIfCancellationRequested();
                    chunkLengths.Add(result.Count);
                    if (result.EndOfMessage)
                    {
                        var message = new byte[chunkLengths.Sum()];
                        var copied = 0;
                        for (int i = 0; i < rented.Count; i++)
                        {
                            Array.Copy(rented[i], 0, message, copied, chunkLengths[i]);
                            copied += chunkLengths[i];
                            _buffer.Return(rented[i]);
                        }

                        rented.Clear();
                        chunkLengths.Clear();

                        var document = JsonDocument.Parse(message, new JsonDocumentOptions()
                        {
                            CommentHandling = JsonCommentHandling.Skip,
                            AllowTrailingCommas = true
                        });
#pragma warning disable 4014
                        Task.Run(() =>
                        {
                            OnMessageReceived?.Invoke(document);
                            document.Dispose();
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
                    OnMessageReceived?.Invoke(new TransportException("Listener failed", ex));
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
                    // ReSharper disable once MethodSupportsCancellation
                    Task.Run(Listen);
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
                OnMessageReceived?.Invoke(new TransportException("Failed to connect", ex));
            }
        }

        public void Dispose()
        {
            _listeningCancellation.Cancel();
            _webSocket?.Dispose();
            _listeningCancellation?.Dispose();
        }
    }
}