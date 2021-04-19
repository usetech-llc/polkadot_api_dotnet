using OneOf;
using Polkadot.Api.Client.Modules.State.Model.Interfaces;
using Polkadot.Api.Client.Modules.State.Model.StorageEntryModifier;
using Polkadot.Api.Client.Modules.State.Model.StorageEntryType;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Modules.State.Model.V12
{
    public class StorageEntryMetadataV12: IStorage
    {
        [Serialize(0)]
        [Utf8StringConverter]
        public string Name { get; set; }
        
        [Serialize(1)]
        [OneOfConverter]
        public OneOf<OptionalModifier, DefaultModifier> Modifier { get; set; }
        
        [Serialize(2)]
        [OneOfConverter]
        public OneOf<Plain, Map, DoubleMap> StorageEntryType { get; set; }
        
        [Serialize(3)]
        [PrefixedArrayConverter]
        public byte[] Default { get; set; }
        
        [Serialize(4)]
        [PrefixedArrayConverter(ItemConverter = typeof(Utf8StringConverter))]
        public string[] Documentation { get; set; }

        public string GetName()
        {
            return Name;
        }

        public OneOf<Plain, Map, DoubleMap> GetStorageType()
        {
            return StorageEntryType;
        }
    }
}