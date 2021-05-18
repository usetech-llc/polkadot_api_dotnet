using OneOf;
using Polkadot.Api.Client.Model;
using Polkadot.BinaryContracts.Events.PhaseEnum;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events
{
    public class EventRecord
    {
        [Serialize(0)]
        public Phase Phase;

        [Serialize(1)]
        [Converter(ConverterType = typeof(EventConverter))]
        public IEvent Event;
        
        [Serialize(2)]
        [PrefixedArrayConverter]
        public Hash256[] Topics;

        public EventRecord()
        {
        }

        public EventRecord(Phase phase, IEvent @event, Hash256[] topics)
        {
            Phase = phase;
            Event = @event;
            Topics = topics;
        }
    }
}