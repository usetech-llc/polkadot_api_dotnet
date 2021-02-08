using System.Collections.Generic;

namespace Polkadot.NodeTypesGenerator
{
    public class Property
    {
        public string ConverterAttribute { get; set; }
        
        public string PropertyName { get; set; }
        
        public string Type { get; set; }
        
        public string OriginalType { get; set; }
    }
}