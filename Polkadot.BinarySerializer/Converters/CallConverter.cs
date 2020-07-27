using System;

namespace Polkadot.BinarySerializer.Converters
{
    public class CallConverter : MetadataIndexConverter
    {
        protected override (byte moduleIndex, byte itemIndex) GetIndex(Type type, IBinarySerializer serializer) => serializer.GetCallIndex(type);

        protected override Type GetCallType(IBinarySerializer deserializer, byte module, byte item) => deserializer.GetCallType(module, item);
    }
}