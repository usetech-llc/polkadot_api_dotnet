using System;
using System.Collections.Generic;

namespace Polkadot.BinarySerializer
{
    public class SerializerSettings
    {
        internal readonly List<(string module, string method, Type type)> KnownCalls =
            new List<(string module, string method, Type type)>();

        internal readonly List<(string module, string @event, Type type)> KnownEvents =
            new List<(string module, string @event, Type type)>();

        public SerializerSettings AddCall<TCall>(string module, string method) where TCall : IExtrinsicCall
        {
            KnownCalls.Add((module, method, typeof(TCall)));
            return this;
        }

        public SerializerSettings AddEvent<TEvent>(string module, string @event) where TEvent : IEvent
        {
            KnownEvents.Add((module, @event, typeof(TEvent)));
            return this;
        }
    }
}