using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace Polkadot.Api.Client.RpcCalls
{
    public interface IRpc
    {
        Task<TResult> Call<TResult>(string method, CancellationToken token,
            params object[] parameters);
        TModule GetModule<TModule>(Func<TModule> moduleFactory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseMethod">The 'method' parameter in subscription response.</param>
        /// <param name="subscribeMethod">Method used to initialize subscription.</param>
        /// <param name="unsubscribeMethod">Method used to terminate subscription.</param>
        /// <param name="onMessage">Message handler.</param>
        /// <param name="keepAlive">Pass true to automatically resubscribe if connection is lost.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="parameters">Parameters of subscribe method.</param>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <returns></returns>
        Task<ISubscription> Subscribe<TMessage>(string responseMethod, string subscribeMethod, string unsubscribeMethod,
            Func<OneOf<TMessage, Exception>, Task> onMessage, bool keepAlive, CancellationToken token,
            params object[] parameters);
    }
}