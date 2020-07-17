using System.IO;

namespace Polkadot.BinarySerializer
{
    public interface IBinarySerializable
    {
        void Serialize(Stream stream, IBinarySerializer serializer);
    }
}