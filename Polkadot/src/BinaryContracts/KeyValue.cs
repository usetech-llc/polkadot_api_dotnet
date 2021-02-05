using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class KeyValue
    {
        [Serialize(0)]
        public byte[] Key { get; set; }
        [Serialize(1)]
        public byte[] Value { get; set; }
    }
}