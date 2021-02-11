using System.Collections.Generic;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.V12
{
    public class EventMetadataV12 : IEventMeta
    {
        [Serialize(0)]
        [Utf8StringConverter]
        public string Name { get; set; }
        
        [Serialize(1)]
        [PrefixedArrayConverter(ItemConverter = typeof(Utf8StringConverter))]
        public string[] Arguments { get; set; }
        
        [Serialize(2)]
        [PrefixedArrayConverter(ItemConverter = typeof(Utf8StringConverter))]
        public string[] Documentation { get; set; }

        public string GetName()
        {
            return Name;
        }

        public IReadOnlyList<string> GetArguments() => Arguments;
    }
}