using OneOf;

namespace Polkadot.NodeTypesGenerator.TypesParser.Types
{
    public class RustType
    {
        public OneOf<RustGeneric, RustTuple, RustSimpleType> Type { get; set; }
    }
}