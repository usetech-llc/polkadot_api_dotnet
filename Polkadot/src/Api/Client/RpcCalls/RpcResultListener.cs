using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Utils;

namespace Polkadot.Api.Client.RpcCalls
{
    internal class RpcResultListener<TResult>
    {
        private readonly ISubstrateClient _substrateClient;
        private readonly long _id;
        private readonly CancellationToken _token;
        private TaskCompletionSource<TResult> _taskCompletionSource;
        private readonly CancellationTokenSource _timeoutCancellation = new();
        
        public Task<TResult> Result { get; }

        public RpcResultListener(ISubstrateClient substrateClient, long id, CancellationToken token, TimeSpan? timeout)
        {
            _substrateClient = substrateClient;
            _id = id;
            _token = CancellationTokenSource.CreateLinkedTokenSource(token, _timeoutCancellation.Token).Token;
            _taskCompletionSource = new();
            Result = _taskCompletionSource.Task;
            _substrateClient.OnMessageReceived += ResponseListener;
            if (timeout != null)
            {
                Task.Delay(timeout.Value, _token).ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(new TimeoutException());
                        _substrateClient.OnMessageReceived -= ResponseListener;
                        _timeoutCancellation.Cancel();
                    }
                });
            }
        }

        public void ResponseListener(OneOf<JsonDocument, Exception> message)
        {
            message.Switch(doc =>
            {
                if (_token.IsCancellationRequested)
                {
                    Interlocked.Exchange(ref _taskCompletionSource, null)?.SetCanceled();
                    _substrateClient.OnMessageReceived -= ResponseListener;
                    return;
                }

                if (!MyResponse(doc))
                {
                    return;
                }
                try
                {
                    if (doc.RootElement.TryGetProperty("result", out var resultElement))
                    {
                        var resultBytes = resultElement.GetString().HexToByteArray();
                        var result = _substrateClient.BinarySerializer.Deserialize<TResult>(resultBytes);
                        Interlocked.Exchange(ref _taskCompletionSource, null)?.SetResult(result);
                        return;
                    }

                    if (doc.RootElement.TryGetProperty("error", out var errorElement))
                    {
                        long? errorCode = 
                            errorElement.TryGetProperty("code", out var codeElement) && codeElement.TryGetInt64(out var c)  ? c : null;
                        var jrpcErrorException = new JrpcErrorException(errorCode, errorElement.Clone());
                        Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(jrpcErrorException);
                        return;
                    }

                    var jrpcDeserializationException = new JrpcDeserializationException(doc.RootElement.Clone());
                    Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(jrpcDeserializationException);
                }
                catch (Exception ex)
                {
                    Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(new JrpcDeserializationException(doc.RootElement.Clone(), ex));
                }
                finally
                {
                    _substrateClient.OnMessageReceived -= ResponseListener;
                    _timeoutCancellation.Cancel();
                }
            }, error =>
            {
                Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(error);
                _substrateClient.OnMessageReceived -= ResponseListener;
                _timeoutCancellation.Cancel();
            });
        }

        private bool MyResponse(JsonDocument doc)
        {
            return doc.RootElement.TryGetProperty("id", out var element) 
                   && element.TryGetInt64(out var messageId) 
                   && messageId == _id;
        }
    }
}