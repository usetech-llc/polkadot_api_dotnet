using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Modules.State.Model.StorageEntryType
{
    public class DoubleMap
    {
        [Serialize(0)]
        public StorageHasher Hasher { get; set; }
        [Serialize(1)]
        [Utf8StringConverter]
        public string Key1 { get; set; }
        [Serialize(2)]
        [Utf8StringConverter]
        public string Key2 { get; set; }
        [Serialize(3)]
        [Utf8StringConverter]
        public string Value { get; set; }
        [Serialize(4)]
        public StorageHasher Key2Hasher { get; set; }
    }
}