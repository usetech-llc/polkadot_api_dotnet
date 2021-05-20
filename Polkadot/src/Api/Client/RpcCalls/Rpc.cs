using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Serialization;
using Polkadot.Utils;

namespace Polkadot.Api.Client.RpcCalls
{
    public class Rpc<TJsonElement> : IRpc where TJsonElement : IJsonElement<TJsonElement>
    {
        private readonly ISubstrateClient<TJsonElement> _substrateClient;
        private readonly ConcurrentDictionary<Type, object> _modules = new();
        private readonly SubscriptionQueue<TJsonElement> _subscriptionQueue;

        public Rpc(ISubstrateClient<TJsonElement> substrateClient)
        {
            _substrateClient = substrateClient;
            _subscriptionQueue = new(_substrateClient);
        }
        
        public Task<TResult> Call<TResult>(string method, CancellationToken token,
            params object[] parameters)
        {
            parameters ??= Array.Empty<object>();
            var id = _substrateClient.NextRequestId();
            var jrpcParameter = new JrpcParameter(id, method, parameters.Select(p => _substrateClient.BinarySerializer.Serialize(p).ToPrefixedHexString()));
            var bytes = _substrateClient.JsonSerializer.Serialize(jrpcParameter);

            var listener = new RpcResultListener<TResult, TJsonElement>(_substrateClient, id, token);
            _substrateClient.Send(bytes, token);

            return listener.Result;
        }

        public TModule GetModule<TModule>(Func<TModule> moduleFactory)
        {
            return (TModule)_modules.GetOrAdd(typeof(TModule), _ => moduleFactory());
        }

        public Task<ISubscription> Subscribe<TMessage>(string responseMethod, string subscribeMethod,
            string unsubscribeMethod, Func<OneOf<TMessage, Exception>, Task> onMessage, bool keepAlive,
            CancellationToken token, params object[] parameters)
        {
            return _subscriptionQueue.Subscribe(responseMethod, subscribeMethod, unsubscribeMethod, onMessage, keepAlive, token, parameters);
        }
    }
}