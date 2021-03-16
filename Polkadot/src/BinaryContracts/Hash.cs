using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts
{
    public class Hash
    {
        [Serialize(0)]
        [FixedSizeArrayConverter(32)]
        public byte[] Value { get; set; }
    }
}