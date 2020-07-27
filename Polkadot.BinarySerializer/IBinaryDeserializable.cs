using System;
using System.IO;

namespace Polkadot.BinarySerializer
{
    public interface IBinaryDeserializable
    {
        object Deserialize(Stream stream, IBinarySerializer serializer);
    }
}