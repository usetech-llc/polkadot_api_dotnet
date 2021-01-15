using System.Collections.Generic;
using OneOf;
using Polkadot.Api.Hashers;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.StorageHasher
{
    public class StorageHasher : IHasher
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<
            Blake2_128,
            Blake2_256,
            Blake2_128Concat,
            Twox128,
            Twox256,
            Twox64Concat,
            Identity> 
            Value { get; set; }

        private IHasher Hasher()
        {
            IHasher Cast<T>(T value) where T : IHasher
            {
                return value;
            }

            return Value.Match(Cast, Cast, Cast, Cast, Cast, Cast, Cast);
        }

        public byte[] Hash(byte[] key)
        {
            return Hasher().Hash(key);
        }

        public byte[] HashMultiple(IEnumerable<byte[]> keys)
        {
            return Hasher().HashMultiple(keys);
        }
    }
}