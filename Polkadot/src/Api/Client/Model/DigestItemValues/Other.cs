using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model.DigestItemValues
{
    /// Some other thing. Unsupported and experimental.
    public class Other
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] Value { get; set; }
    }
}