using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.V12
{
    public class FunctionArgumentMetadataV12
    {
        [Serialize(0)]
        [Utf8StringConverter]
        public string Name { get; set; }
        [Serialize(1)]
        [Utf8StringConverter]
        public string Ty { get; set; }
    }
}