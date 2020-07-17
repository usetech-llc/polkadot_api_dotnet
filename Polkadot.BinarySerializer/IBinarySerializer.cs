using System.Collections.Generic;
using System.IO;

namespace Polkadot.BinarySerializer
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T value);
        void Serialize<T>(T value, Stream stream);
        long ReadLong(IEnumerator<byte> input);
    }
}