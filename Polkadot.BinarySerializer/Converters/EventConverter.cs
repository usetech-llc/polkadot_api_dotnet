using System;

namespace Polkadot.BinarySerializer.Converters
{
    public class EventConverter : MetadataIndexConverter
    {
        protected override (byte moduleIndex, byte itemIndex) GetIndex(Type type, IBinarySerializer serializer) => serializer.GetEventIndex(type);

        protected override Type GetCallType(IBinarySerializer deserializer, byte module, byte item) => deserializer.GetEventType(module, item);
    }
}