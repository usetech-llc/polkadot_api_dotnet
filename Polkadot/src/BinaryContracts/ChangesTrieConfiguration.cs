using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class ChangesTrieConfiguration : IExtrinsicCall
    {
        /// Interval (in blocks) at which level1-digests are created. Digests are not
        /// created when this is less or equal to 1.
        [Serialize(0)]
        public uint DigestInterval { get; set; }
        /// Maximal number of digest levels in hierarchy. 0 means that digests are not
        /// created at all (even level1 digests). 1 means only level1-digests are created.
        /// 2 means that every digest_interval^2 there will be a level2-digest, and so on.
        /// Please ensure that maximum digest interval (i.e. digest_interval^digest_levels)
        /// is within `u32` limits. Otherwise you'll never see digests covering such intervals
        /// && maximal digests interval will be truncated to the last interval that fits
        /// `u32` limits.
        [Serialize(1)]
        public uint DigestLevels { get; set; }
    }
}