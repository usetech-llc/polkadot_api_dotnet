using System;
using System.Collections.Generic;
using System.IO;
using Extensions.Data;

namespace Polkadot.Api.Hashers
{
    public class Twox64Concat: IHasher
    {
        public byte[] Hash(byte[] key)
        {
            var serializer = new BinarySerializer.BinarySerializer();
            var xxhash1 = XXHash.XXH64(key, 0, key.Length, 0);
            using var hashStream = new MemoryStream();
            serializer.Serialize(xxhash1, hashStream);
            hashStream.Write(key, 0, key.Length);
            return hashStream.ToArray();
        }

        public byte[] HashMultiple(IEnumerable<byte[]> keys)
        {
            return Utils.Hash.HashThenConcat(keys, Hash);
        }
    }
}