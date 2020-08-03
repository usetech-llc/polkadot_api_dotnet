using System.Linq;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts
{
    public class DoubleMapKey
    {
        public static DoubleMapKey<TKey1, TKey2> Create<TKey1, TKey2>(TKey1 key1, TKey2 key2)
        {
            return new DoubleMapKey<TKey1, TKey2>(key1, key2);
        }
    }
    
    /// <summary>
    /// For storage requests with hashed params.
    /// </summary>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    public class DoubleMapKey<TKey1, TKey2> : ITypeCreate
    {
        public readonly TKey1 Key1 = default!;
        public readonly TKey2 Key2 = default!;

        public DoubleMapKey()
        {
        }

        public DoubleMapKey(TKey1 key1, TKey2 key2)
        {
            Key1 = key1;
            Key2 = key2;
        }

        public byte[] GetTypeEncoded(IBinarySerializer serializer)
        {
            return MapKey.Create(Key1).HashKey(serializer)
                .Concat(MapKey.Create(Key2).HashKey(serializer))
                .ToArray();
        }
    }
}