using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class BlockNumber
    {
        [Serialize(0)]
        public uint Number { get; set; }
    }
}