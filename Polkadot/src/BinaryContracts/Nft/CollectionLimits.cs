using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Nft
{
    public class CollectionLimits
    {
        [Serialize(0)]
        public uint AccountTokenOwnershipLimit { get; set; }
        [Serialize(1)]
        public uint SponsoredMintSize { get; set; }
        [Serialize(2)]
        public uint TokenLimit { get; set; }
        [Serialize(3)]
        public uint SponsorTimeout { get; set; }
    }
}