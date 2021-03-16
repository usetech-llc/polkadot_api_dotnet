using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.V12
{
    public class ErrorMetadataV12 : IErrorMeta
    {
        [Serialize(0)]
        [Utf8StringConverter]
        public string Name { get; set; }
        
        [Serialize(0)]
        [PrefixedArrayConverter(ItemConverter = typeof(Utf8StringConverter))]
        public string[] Documentation { get; set; }

        public string GetName()
        {
            return Name;
        }
    }
}