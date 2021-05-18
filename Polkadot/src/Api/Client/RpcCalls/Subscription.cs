using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using OneOf;
using Polkadot.Api.Client.Serialization;

namespace Polkadot.Api.Client.RpcCalls
{
    internal class Subscription<TMessage, TJsonElement> : ISubscription where TJsonElement : IJsonElement<TJsonElement>
    {
        private readonly ChannelReader<OneOf<TJsonElement, Exception>> _reader;
        private readonly Func<OneOf<TMessage, Exception>, Task> _handler;
        private readonly Func<Task> _unsubscribe;

        public Subscription(ChannelReader<OneOf<TJsonElement, Exception>> reader, Func<OneOf<TMessage, Exception>, Task> handler, Func<Task> unsubscribe)
        {
            _reader = reader;
            _handler = handler;
            _unsubscribe = unsubscribe;
            Task.Run(Listen);
        }

        private async Task Listen()
        {
            while (true)
            {
                try
                {
                    var element = await _reader.ReadAsync();
                    var handler = element.Match<Task>(async e =>
                    {
                        var message = await e.DeserializeObject<TMessage>();
                        await _handler(message);
                    }, e => _handler(e));
                    await handler;
                }
                catch (ChannelClosedException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    try
                    {
                        await _handler(ex);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public Task Unsubscribe()
        {
            return _unsubscribe();
        }

        public void Dispose()
        {
            Unsubscribe().GetAwaiter().GetResult();
        }
    }
}