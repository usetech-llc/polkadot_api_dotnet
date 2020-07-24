using System;
using System.IO;

namespace Polkadot.BinarySerializer
{
    public interface IBinaryConverter
    {
        void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters);
        object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters);
    }
}