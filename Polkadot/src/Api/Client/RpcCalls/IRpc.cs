using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.RpcCalls
{
    public interface IRpc
    {
        Task<TResult> Call<TResult>(string method, IEnumerable<object> parameters = null, CancellationToken token = default, TimeSpan? timeout = default);
        TModule GetModule<TModule>(Func<TModule> moduleFactory);
    }
}