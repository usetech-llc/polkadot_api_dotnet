using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType
{
    public class Map
    {
        [Serialize(0)]
        public StorageHasher.StorageHasher Hasher { get; set; }
        
        [Serialize(1)]
        [Utf8StringConverter]
        public string Key { get; set; }
        
        [Serialize(2)]
        [Utf8StringConverter]
        public string Value { get; set; }
        
        // is_linked flag previously, unused now to keep backwards compat
        [Serialize(3)]
        public bool Unused { get; set; }
    }
}