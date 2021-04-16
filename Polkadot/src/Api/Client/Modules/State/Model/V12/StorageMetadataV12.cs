using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.V12
{
    public class StorageMetadataV12
    {
        /// The common prefix used by all storage entries.
        [Serialize(0)]
        [Utf8StringConverter]
        public string Prefix { get; set; }
        
        [Serialize(1)]
        [PrefixedArrayConverter]
        public StorageEntryMetadataV12[] Entries { get; set; }

    }
}