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

        public static Task<TResult> CallWithOptionalParam<TResult, TParam1, TParam2>(this IRpc rpc, string method, CancellationToken token,
            TParam1 parameter1, TParam2 parameter2)
        {
            if (EqualityComparer<TParam2>.Default.Equals(parameter2, default))
            {
                if (EqualityComparer<TParam1>.Default.Equals(parameter1, default))
                {
                    return rpc.Call<TResult>(method, token);
                }

                return rpc.Call<TResult>(method, token, parameter1);
            }
            return rpc.Call<TResult>(method, token, parameter1, parameter2);
        }

        public static Task<TResult> CallWithOptionalParam<TResult, TParam1, TParam2, TParam3>(this IRpc rpc, string method, CancellationToken token,
            TParam1 parameter1, TParam2 parameter2, TParam3 parameter3)
        {
            if (EqualityComparer<TParam3>.Default.Equals(parameter3, default))
            {
                if (EqualityComparer<TParam2>.Default.Equals(parameter2, default))
                {
                    if (EqualityComparer<TParam1>.Default.Equals(parameter1, default))
                    {
                        return rpc.Call<TResult>(method, token);
                    }

                    return rpc.Call<TResult>(method, token, parameter1);
                }
                return rpc.Call<TResult>(method, token, parameter1, parameter2);
            }
            return rpc.Call<TResult>(method, token, parameter1, parameter2, parameter3);
        }
    }
}