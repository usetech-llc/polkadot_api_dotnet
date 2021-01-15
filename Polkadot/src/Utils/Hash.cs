using Extensions.Data;
using Polkadot.DataStructs;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Polkadot.BinarySerializer;

namespace Polkadot.Utils
{
    public class Hash
    {
        public static byte[] ConcatThenHash(IEnumerable<byte[]> input, Func<byte[], byte[]> hashFunction)
        {
            var collection = input.AsCollection();
            if (collection.Count == 1)
            {
                return hashFunction(collection.First());
            }
            
            var size = 0;
            foreach (var array in collection)
            {
                size += array.Length;
            }

            var concat = ArrayPool<byte>.Shared.Rent(size);
            var offset = 0;
            foreach (var array in collection)
            {
                Array.Copy(array, 0, concat, offset, array.Length);
                offset += array.Length;
            }

            var hash = hashFunction(concat);
            ArrayPool<byte>.Shared.Return(concat);
            return hash;
        }

        public static byte[] HashThenConcat(IEnumerable<byte[]> input, Func<byte[], byte[]> hashFunction)
        {
            var collection = input.AsCollection();
            if (collection.Count == 1)
            {
                return hashFunction(collection.First());
            }

            var hash = new List<byte>();
            foreach (var array in collection)
            {
                hash.AddRange(hashFunction(array));
            }

            return hash.ToArray();
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
