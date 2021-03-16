using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class Weight
    {
        [Serialize(0)]
        public ulong Value { get; set; }
    }
}