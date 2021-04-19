using Polkadot.Api.Client.Modules.State.Model.Interfaces;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Modules.State.Model.V12
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