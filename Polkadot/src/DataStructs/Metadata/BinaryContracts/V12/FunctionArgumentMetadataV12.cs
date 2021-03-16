using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.V12
{
    public class FunctionArgumentMetadataV12 : ICallArgument
    {
        [Serialize(0)]
        [Utf8StringConverter]
        public string Name { get; set; }

        [Serialize(1)]
        [Utf8StringConverter]
        public string Ty { get; set; }

        public string Type => Ty;
    }
}