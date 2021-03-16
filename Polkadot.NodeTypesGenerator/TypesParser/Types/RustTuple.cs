using System.Collections.Generic;

namespace Polkadot.NodeTypesGenerator.TypesParser.Types
{
    public class RustTuple
    {
        public List<RustType> RustTypes { get; set; } = new();
    }
}