using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [BinaryJsonConverter]
    public class PrefixedStorageKey
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] Key { get; set; }

        public static implicit operator byte[](PrefixedStorageKey key)
        {
            return key.Key;
        }

        public static implicit operator PrefixedStorageKey(byte[] key)
        {
            return new() {Key = key};
        }
    }
}