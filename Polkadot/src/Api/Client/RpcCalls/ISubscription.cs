using System;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.RpcCalls
{
    public interface ISubscription : IDisposable
    {
        Task Unsubscribe();
    }
}