using System.IO;
using System.Linq;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.DataStructs.Metadata;
using Polkadot.Utils;

namespace Polkadot.BinaryContracts
{
    public class DoubleMapKey
    {
        public static DoubleMapKey<TKey> Create<TKey>(TKey key)
        {
            return new DoubleMapKey<TKey>(key);
        }
    }
    
    /// <summary>
    /// For storage requests with hashed params.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DoubleMapKey<TKey> : ITypeCreate
    {
        public TKey Key = default!;

        public DoubleMapKey()
        {
        }

        public DoubleMapKey(TKey key)
        {
            Key = key;
        }

        public byte[] GetTypeEncoded(IBinarySerializer serializer)
        {
            using var ms = new MemoryStream();
            serializer.Serialize(Key, ms);
            var serializedKey = ms.ToArray();
            var hashedKey = Metadata.GetStorageKey(Hasher.BLAKE2, serializedKey, serializedKey.Length, serializer).HexToByteArray();
            return hashedKey.Concat(serializedKey).ToArray();
        }
    }
}