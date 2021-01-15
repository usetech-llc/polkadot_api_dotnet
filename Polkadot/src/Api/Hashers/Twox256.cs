using System.Collections.Generic;
using System.IO;
using Extensions.Data;

namespace Polkadot.Api.Hashers
{
    public class Twox256 : IHasher
    {
        public byte[] Hash(byte[] key)
        {
            var serializer = new BinarySerializer.BinarySerializer();
            var xxhash1 = XXHash.XXH64(key, 0, key.Length, 0);
            var xxhash2 = XXHash.XXH64(key, 0, key.Length, 1);
            var xxhash3 = XXHash.XXH64(key, 0, key.Length, 2);
            var xxhash4 = XXHash.XXH64(key, 0, key.Length, 3);
            using var ms = new MemoryStream();
            serializer.Serialize(xxhash1, ms);
            serializer.Serialize(xxhash2, ms);
            serializer.Serialize(xxhash3, ms);
            serializer.Serialize(xxhash4, ms);
            return ms.ToArray();
        }

        public byte[] HashMultiple(IEnumerable<byte[]> keys)
        {
            return Utils.Hash.HashThenConcat(keys, Hash);
        }
    }
}