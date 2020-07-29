using Extensions.Data;
using Polkadot.DataStructs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Polkadot.BinarySerializer;

namespace Polkadot.Utils
{
    public class Hash
    {
        public static byte[] GetStorageKey(Hasher hasher, byte[] bytes, int length, IBinarySerializer serializer)
        {
            return hasher switch
            {
                Hasher.XXHASH => XxHash(bytes, length, serializer),
                Hasher.BLAKE2 => Blake2(bytes, length),
                _ => Array.Empty<byte>()
            };
        }

        private static byte[] Blake2(byte[] bytes, int length)
        {
            var config = new Blake2Core.Blake2BConfig { OutputSizeInBytes = 16 };
            return Blake2Core.Blake2B.ComputeHash(bytes, 0, length, config);
        }

        private static byte[] XxHash(byte[] bytes, int length, IBinarySerializer serializer)
        {
            var xxhash1 = XXHash.XXH64(bytes, 0, length, 0);
            var xxhash2 = XXHash.XXH64(bytes, 0, length, 1);
            using var ms = new MemoryStream();
            serializer.Serialize(xxhash1, ms);
            serializer.Serialize(xxhash2, ms);
            return ms.ToArray();
        }
    }
}
