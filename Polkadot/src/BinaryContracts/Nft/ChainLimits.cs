using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Nft
{
    public class ChainLimits
    {
        [Serialize(0)]
        public uint CollectionNumbersLimit { get; set; }
        [Serialize(1)]
        public uint AccountTokenOwnershipLimit { get; set; }
        [Serialize(2)]
        public ulong CollectionsAdminsLimit { get; set; }
        [Serialize(3)]
        public uint CustomDataLimit { get; set; }
        [Serialize(4)]
        public uint NftSponsorTimeout { get; set; }
        [Serialize(5)]
        public uint FungibleSponsorTimeout { get; set; }
        [Serialize(6)]
        public uint ReFungibleSponsorTimeout { get; set; }
    }
}