using System.Text;
using Polkadot.Api.Client.Modules.State.Model.Interfaces;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Modules.State.Model.V12
{
    public class ModuleConstantMetadataV12 : IConstantMeta
    {

        [Serialize(0)]
        [Utf8StringConverter]
        public string Name { get; set; }
        
        [Serialize(1)]
        [Utf8StringConverter]
        public string Ty { get; set; }
        
        [Serialize(2)]
        [PrefixedArrayConverter]
        public byte[] Value { get; set; }
        
        [Serialize(3)]
        [PrefixedArrayConverter(ItemConverter = typeof(Utf8StringConverter))]
        public string[] Documentation { get; set; }

        public string GetName()
        {
            return Name;
        }

        public string GetValue()
        {
            return Encoding.UTF8.GetString(Value);
        }

        public byte[] GetValueBytes()
        {
            return Value;
        }
    }
}