using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs
{
    public class PublicKey
    {
        [Serialize(0)]
        [FixedSizeArrayConverter(32)]
        public byte[] Bytes { get; set; }
    }
}