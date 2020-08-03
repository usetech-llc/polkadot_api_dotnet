using System.Collections.Generic;
using System.IO;
using System.Linq;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.DataStructs.Metadata;
using Polkadot.Utils;

namespace Polkadot.BinaryContracts
{
    public class MapKey
    {
        public static MapKey<TKey> Create<TKey>(TKey key)
        {
            return new MapKey<TKey>(key);
        }
    }
    
    /// <summary>
    /// For storage requests with hashed params.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class MapKey<TKey> : ITypeCreate
    {
        public readonly TKey Key = default!;

        public MapKey()
        {
        }

        public MapKey(TKey key)
        {
            Key = key;
        }

        public byte[] GetTypeEncoded(IBinarySerializer serializer)
        {
            return HashKey(serializer).ToArray();
        }

        public IEnumerable<byte> HashKey(IBinarySerializer serializer)
        {
            using var ms = new MemoryStream();
            serializer.Serialize(Key, ms);
            var serializedKey = ms.ToArray();
            var hashedKey = Metadata.GetStorageKey(Hasher.BLAKE2, serializedKey, serializedKey.Length, serializer).HexToByteArray();
            return hashedKey.Concat(serializedKey);
        }
    }
}