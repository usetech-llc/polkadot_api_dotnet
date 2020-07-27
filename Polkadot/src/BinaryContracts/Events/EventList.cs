using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events
{
    public class EventList
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public EventRecord[] Events;

        public EventList()
        {
        }

        public EventList(EventRecord[] events)
        {
            Events = events;
        }
    }
}