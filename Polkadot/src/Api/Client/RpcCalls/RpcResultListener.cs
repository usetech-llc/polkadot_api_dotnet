using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Serialization;
using Polkadot.Utils;

namespace Polkadot.Api.Client.RpcCalls
{
    internal class RpcResultListener<TResult, TJsonElement> where TJsonElement : IJsonElement<TJsonElement>
    {
        private readonly ISubstrateClient<TJsonElement> _substrateClient;
        private readonly long _id;
        private readonly CancellationToken _token;
        private TaskCompletionSource<TResult> _taskCompletionSource;
        private readonly CancellationTokenSource _timeoutCancellation = new();
        
        public Task<TResult> Result { get; }

        public RpcResultListener(ISubstrateClient<TJsonElement> substrateClient, long id, CancellationToken token)
        {
            _substrateClient = substrateClient;
            _id = id;
            _token = CancellationTokenSource.CreateLinkedTokenSource(token, _timeoutCancellation.Token).Token;
            _taskCompletionSource = new();
            Result = _taskCompletionSource.Task;
            _substrateClient.MessageReceived += ResponseListener;
            if (substrateClient.RpcTimeout != null)
            {
                Task.Delay(substrateClient.RpcTimeout.Value, _token).ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(new TimeoutException());
                        _substrateClient.MessageReceived -= ResponseListener;
                        _timeoutCancellation.Cancel();
                    }
                });
            }
        }

        public void ResponseListener(OneOf<TJsonElement, Exception> message)
        {
            message.Switch(element =>
            {
                if (_token.IsCancellationRequested)
                {
                    Interlocked.Exchange(ref _taskCompletionSource, null)?.SetCanceled();
                    _substrateClient.MessageReceived -= ResponseListener;
                    return;
                }

                if (!MyResponse(element))
                {
                    return;
                }
                
                try
                {
                    if (element.TryGetProperty("result", out var resultElement))
                    {
                        var result = resultElement.DeserializeObject<TResult>().GetAwaiter().GetResult();
                        Interlocked.Exchange(ref _taskCompletionSource, null)?.SetResult(result);
                        return;
                    }

                    if (element.TryGetProperty("error", out var errorElement))
                    {
                        long? errorCode = 
                            errorElement.TryGetProperty("code", out var codeElement) && codeElement.TryGetLong(out var c)  ? c : null;
                        var jrpcErrorException = new JrpcErrorException<TJsonElement>(errorCode, errorElement.Clone());
                        Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(jrpcErrorException);
                        return;
                    }

                    var jrpcDeserializationException = new JrpcDeserializationException<TJsonElement>(element.Clone(), typeof(TResult));
                    Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(jrpcDeserializationException);
                }
                catch (Exception ex)
                {
                    Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(new JrpcDeserializationException<TJsonElement>(element.Clone(), typeof(TResult), ex));
                }
                finally
                {
                    _substrateClient.MessageReceived -= ResponseListener;
                    _timeoutCancellation.Cancel();
                }
            }, error =>
            {
                Interlocked.Exchange(ref _taskCompletionSource, null)?.SetException(error);
                _substrateClient.MessageReceived -= ResponseListener;
                _timeoutCancellation.Cancel();
            });
        }

        private bool MyResponse(TJsonElement doc)
        {
            return doc.TryGetProperty("id", out var element) 
                   && element.TryGetLong(out var messageId) 
                   && messageId == _id;
        }
    }
}