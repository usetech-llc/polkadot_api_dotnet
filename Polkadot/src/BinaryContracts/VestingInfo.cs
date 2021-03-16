using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class VestingInfo
    {
        /// Locked amount at genesis.
        [Serialize(0)]
        public Balance Locked { get; set; }
        /// Amount that gets unlocked every block after `starting_block`.
        [Serialize(1)]
        public Balance PerBlock { get; set; }
        /// Starting block for unlocking(vesting).
        [Serialize(2)]
        public BlockNumber StartingBlock { get; set; }
    }
}