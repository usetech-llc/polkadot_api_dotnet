using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polkadot.Api.Client.RpcCalls;

namespace Polkadot.Extensions
{
    public static class RpcExtensions
    {
        public static Task<TResult> CallWithOptionalParam<TResult, TParam>(this IRpc rpc, string method, CancellationToken token,
            TParam parameter)
        {
            if (EqualityComparer<TParam>.Default.Equals(parameter, default))
            {
                return rpc.Call<TResult>(method, token);
            }

            return rpc.Call<TResult>(method, token, parameter);
        }
    }
}