using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polkadot.Utils
{
    public static class TaskExtensions
    {
        public static T Sync<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
        
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan interval)
        {
            using var tokenSource = new CancellationTokenSource();
            var t = await Task.WhenAny(task, Task.Delay(interval, tokenSource.Token));
            if (t != task)
            {
                throw new TimeoutException();
            }
            return await task;
        } 
    }
}