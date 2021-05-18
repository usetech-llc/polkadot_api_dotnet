using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Exceptions;
using Polkadot.Api.Client.Serialization;
using Polkadot.Extensions;
using Polkadot.Utils;

namespace Polkadot.Api.Client.RpcCalls
{
    internal class SubscriptionQueue<TJsonElement> where TJsonElement : IJsonElement<TJsonElement>
    {
        private readonly ISubstrateClient<TJsonElement> _client;
        private readonly ConcurrentDictionary<(string Method, string SubscriptionId), Channel<OneOf<TJsonElement, Exception>>>
            _subscribers = new();

        private readonly Dictionary<string, string> _unsubscribeMethods = new();
        private readonly Dictionary<Guid, (string Method, string SubscribeMethod, string UnsubscribeMethod, IReadOnlyCollection<object> Parameters)> _keepAliveSubscriptionInfo = new();
        private readonly ConcurrentDictionary<Guid, string> _callIdToSubscriptionId = new();
        private readonly HashSet<(string Method, string SubscriptionId)> _handledSubscriptions = new();
        private CancellationTokenSource _reconnectCancellation = new();

        public SubscriptionQueue(ISubstrateClient<TJsonElement> client)
        {
            _client = client;
            _client.MessageReceived += ClientMessageReceived;
        }

        private void ClientMessageReceived(OneOf<TJsonElement, Exception> message)
        {
            message.Switch(element =>
                {
                    var method = GetMethod(element);
                    if (method == null)
                    {
                        return;
                    }

                    var subscriptionParams = GetSubscription(element);
                    if (subscriptionParams == null)
                    {
                        return;
                    }

                    if (!subscriptionParams.Value.@params.TryGetProperty("result", out var result))
                    {
                        return;
                    }

                    var channel = GetChannel((method, subscriptionParams.Value.subscriptionId));
                    channel.Writer.WriteAsync(result.Clone());
                    if (!_handledSubscriptions.Contains((method, subscriptionParams.Value.subscriptionId)) && _client.RpcTimeout.HasValue)
                    {
                        Task.Delay(_client.RpcTimeout.Value)
                            .ContinueWith(t =>
                            {
                                if (!_handledSubscriptions.Contains((method, subscriptionParams.Value.subscriptionId)))
                                {
                                    channel.Writer.WriteAsync(new TimeoutException());
                                    return Unsubscribe(method, subscriptionParams.Value.subscriptionId, _unsubscribeMethods[method]);
                                }

                                return Task.CompletedTask;
                            });
                    }
                },
                ex =>
                {
                    foreach (var pair in _subscribers.ToList())
                    {
                        pair.Value.Writer.WriteAsync(ex);
                    }

                    if (ex.OfType<TransportException>().Any(e => e.IsAnyDisconnectedException()))
                    {
                        Task.Run(Resubscribe);
                    }
                }
            );
        }

        // Method is what you get from websocket once subscribed
        // SubscribeMethod is method you call to initiate subscription and
        // Unsubscribe method is method to call to unsubscribe.
        public async Task<ISubscription> Subscribe<TMessage>(string method, string subscribeMethod, string unsubscribeMethod, Func<OneOf<TMessage, Exception>, Task> handler, bool keepAlive, IReadOnlyCollection<object> parameters = null, CancellationToken token = default)
        {
            var callId = Guid.NewGuid();
            if (keepAlive)
            {
                _keepAliveSubscriptionInfo[callId] = (Method: method, SubscribeMethod: subscribeMethod, UnsubscribeMethod: unsubscribeMethod, Parameters: parameters);
            }

            _unsubscribeMethods[method] = unsubscribeMethod;
            var subscriptionId = await _client.Rpc.Call<string>(subscribeMethod, parameters, token);
            token.ThrowIfCancellationRequested();
            var subscriptionKey = (method, subscriptionId);
            _handledSubscriptions.Add(subscriptionKey);
            var channel = GetChannel(subscriptionKey);

            if (keepAlive)
            {
                UpdateKeepAliveSubscriptionId(callId, subscriptionId);
            }

            var subscription = new Subscription<TMessage, TJsonElement>(channel.Reader, handler, () =>
            {
                if (keepAlive)
                {
                    return StopResubscribingAndUnsubscribe(callId);
                }

                return Unsubscribe(method, subscriptionId, unsubscribeMethod);
            });
            return subscription;
        }

        private Channel<OneOf<TJsonElement, Exception>> GetChannel((string method, string subscriptionId) subscriptionKey)
        {
            return _subscribers.GetOrAdd(subscriptionKey, key => Channel.CreateUnbounded<OneOf<TJsonElement, Exception>>());
        }

        private void UpdateKeepAliveSubscriptionId(Guid callId, string subscriptionId)
        {
            if (!_keepAliveSubscriptionInfo.TryGetValue(callId, out var subscriptionInfo))
            {
                return;
            }

            string oldSubscription = null;
            _callIdToSubscriptionId.AddOrUpdate(callId, subscriptionId, (_, old) =>
            {
                oldSubscription = old;
                return subscriptionId;
            });

            if (oldSubscription != null)
            {
                if (_subscribers.TryRemove((subscriptionInfo.Method, oldSubscription), out var channel))
                {
                    Channel<OneOf<TJsonElement, Exception>> alreadyCreatedChannel = null;
                    _subscribers.AddOrUpdate((subscriptionInfo.Method, subscriptionId), channel, (_, a) =>
                    {
                        alreadyCreatedChannel = a;
                        return channel;
                    });
                    if (alreadyCreatedChannel != null)
                    {
                        try
                        {
                            while (alreadyCreatedChannel.Reader.TryRead(out var i))
                            {
                                channel.Writer.WriteAsync(i);
                            }
                        }
                        catch (Exception ex)
                        {
                            channel.Writer.WriteAsync(ex);
                        }
                    }
                }
                Task.Run(() => Unsubscribe(subscriptionInfo.Method, oldSubscription, subscriptionInfo.UnsubscribeMethod));
            }
        }

        private async Task Resubscribe()
        {
            var myCts = new CancellationTokenSource();
            var cts = Interlocked.Exchange(ref _reconnectCancellation, myCts);
            cts.Cancel();

            var subscriptionRemap = await Task.WhenAll(_keepAliveSubscriptionInfo.Select(async reconnect =>
            {
                var newSubscribeId = await _client.Rpc.Call<string>(reconnect.Value.SubscribeMethod, reconnect.Value.Parameters, cts.Token);
                return (Id: reconnect.Key, NewId: newSubscribeId);
            }));
            if (cts.Token.IsCancellationRequested)
            {
                return;
            }

            foreach (var (id, newId) in subscriptionRemap)
            {
                UpdateKeepAliveSubscriptionId(id, newId);
            }
        }

        private Task Unsubscribe(string method, string subscriptionId, string unsubscribeMethod)
        {
            if (_subscribers.TryRemove((method, subscriptionId), out var channel))
            {
                channel.Writer.Complete();
            }
            
            _handledSubscriptions.Remove((method, subscriptionId));
            return _client.Rpc.Call<bool>(unsubscribeMethod, new []{subscriptionId});
        }

        public Task StopResubscribingAndUnsubscribe(Guid callId)
        {
            if (_keepAliveSubscriptionInfo.TryGetValue(callId, out var subscriptionInfo) && _callIdToSubscriptionId.TryRemove(callId, out var subscriptionId))
            {
                _keepAliveSubscriptionInfo.Remove(callId);
                return Unsubscribe(subscriptionInfo.Method, subscriptionId, subscriptionInfo.UnsubscribeMethod);
            }

            return Task.CompletedTask;
        }

        private (TJsonElement @params, string subscriptionId)? GetSubscription(TJsonElement element)
        {
            if (element.TryGetProperty("params", out var @params)
                && @params.TryGetProperty("subscription", out var subscription)
                && subscription.TryGetString(out var subscriptionId)
                && subscriptionId != null)
            {
                return (@params, subscriptionId);
            }

            return null;
        }

        private string GetMethod(TJsonElement element)
        {
            return element.TryGetProperty("method", out var method) ? method.GetStringOrNull() : null;
        }
    }
}