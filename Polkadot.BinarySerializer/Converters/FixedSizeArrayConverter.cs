using System;
using System.IO;
using Polkadot.BinarySerializer;

namespace Polkadot.BinarySerializer.Converters
{
    public class FixedSizeArrayConverter : BaseArrayConverter
    {
        public override void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] param)
        {
            serializer.Serialize(value, stream);
        }

        public override object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] param)
        {
            return DeserializeArray(type, stream, deserializer, param, (int) param[2]);
        }
    }
}