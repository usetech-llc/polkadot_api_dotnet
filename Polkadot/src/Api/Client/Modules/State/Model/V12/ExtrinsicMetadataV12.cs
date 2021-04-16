using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.V12
{
    public class ExtrinsicMetadataV12
    {
        /// Extrinsic version.
        [Serialize(0)]
        public byte Version { get; set; }
        
        /// The signed extensions in the order they appear in the extrinsic.
        [Serialize(1)]
        [PrefixedArrayConverter(ItemConverter = typeof(Utf8StringConverter))]
        public string[] SignedExtensions { get; set; }

    }
}