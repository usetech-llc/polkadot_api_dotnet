using System;
using System.Collections.Generic;
using System.IO;
using Extensions.Data;

namespace Polkadot.Api.Hashers
{
    public class Twox128 : IHasher
    {
        public byte[] Hash(byte[] key)
        {
            var serializer = new BinarySerializer.BinarySerializer();
            var xxhash1 = XXHash.XXH64(key, 0, key.Length, 0);
            var xxhash2 = XXHash.XXH64(key, 0, key.Length, 1);
            using var ms = new MemoryStream();
            serializer.Serialize(xxhash1, ms);
            serializer.Serialize(xxhash2, ms);
            return ms.ToArray();
        }

        public byte[] HashMultiple(IEnumerable<byte[]> keys)
        {
            return Utils.Hash.HashThenConcat(keys, Hash);
        }
    }
}