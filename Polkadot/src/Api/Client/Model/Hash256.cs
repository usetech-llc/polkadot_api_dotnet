using Newtonsoft.Json;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    [BinaryJsonConverter]
    public class Hash256
    {
        [Serialize(0)]
        [FixedSizeArrayConverter(32)]
        public byte[] Value { get; set; }
    }
}