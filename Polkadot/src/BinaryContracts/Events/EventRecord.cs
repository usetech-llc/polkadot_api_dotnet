using Polkadot.BinaryContracts.Events.Phase;
using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events
{
    public class EventRecord
    {
        [Serialize(0)]
        [Converter(ConverterType = typeof(OneOfEnumConverter))]
        public OneOf<ApplyExtrinsic, Finalization, Initialization> Phase;

        [Serialize(1)]
        [Converter(ConverterType = typeof(EventConverter))]
        public IEvent Event;
        
        [Serialize(2)]
        [PrefixedArrayConverter]
        public ITopics[] Topics;

        public EventRecord()
        {
        }

        public EventRecord(OneOf<ApplyExtrinsic, Finalization, Initialization> phase, IEvent @event, ITopics[] topics)
        {
            Phase = phase;
            Event = @event;
            Topics = topics;
        }
    }
}