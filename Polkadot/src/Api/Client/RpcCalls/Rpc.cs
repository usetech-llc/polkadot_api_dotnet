using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Utils;

namespace Polkadot.Api.Client.RpcCalls
{
    public class Rpc : IRpc
    {
        private readonly ISubstrateClient _substrateClient;
        private readonly Dictionary<Type, object> _modules = new();

        public Rpc(ISubstrateClient substrateClient)
        {
            _substrateClient = substrateClient;
        }
        
        public Task<TResult> Call<TResult>(string method, IEnumerable<object> parameters = null, CancellationToken token = default, TimeSpan? timeout = default)
        {
            parameters ??= Array.Empty<object>();
            var id = _substrateClient.NextRequestId();
            var bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                id = id,
                method = method,
                jsonrpc = "2.0",
                @params = parameters.Select(p => _substrateClient.BinarySerializer.Serialize(p).ToPrefixedHexString())
            });

            var listener = new RpcResultListener<TResult>(_substrateClient, id, token, timeout);
            _substrateClient.Send(bytes, token);

            return listener.Result;
        }

        public TModule GetModule<TModule>(Func<TModule> moduleFactory)
        {
            if (!_modules.TryGetValue(typeof(TModule), out var module))
            {
                module = moduleFactory();
                _modules[typeof(TModule)] = module;
            }
            
            return (TModule) module;
        }
    }
}