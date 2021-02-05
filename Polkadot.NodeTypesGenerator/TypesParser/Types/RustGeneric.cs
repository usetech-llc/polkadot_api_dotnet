using System.Collections.Generic;

namespace Polkadot.NodeTypesGenerator.TypesParser.Types
{
    public class RustGeneric
    {
        public List<RustType> GenericParams { get; set; }
        
        public string GenericName { get; set; }
    }
}