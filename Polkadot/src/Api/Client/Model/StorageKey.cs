using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    [BinaryJsonConverter]
    public class StorageKey
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] Key { get; set; }

        public static implicit operator byte[](StorageKey key)
        {
            return key.Key;
        }

        public static implicit operator StorageKey(byte[] key)
        {
            return new() {Key = key};
        }
    }
}