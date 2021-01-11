using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType
{
    public class Plain
    {
        [Serialize(0)]
        [Utf8StringConverter]
        public string Value { get; set; }
    }
}