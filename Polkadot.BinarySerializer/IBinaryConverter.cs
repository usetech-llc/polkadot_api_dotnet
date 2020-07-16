using System.IO;

namespace Polkadot.BinarySerializer
{
    public interface IBinaryConverter
    {
        void Serialize(Stream stream, object value, IBinarySerializer serializer);
    }
}