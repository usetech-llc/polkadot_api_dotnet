using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class BountyIndex
    {
        [Serialize(0)]
        public uint Value { get; set; }
    }
}