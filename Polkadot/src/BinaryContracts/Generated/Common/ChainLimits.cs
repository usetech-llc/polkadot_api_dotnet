using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class ChainLimits
    {
        // Rust type "u32"
        [Serialize(0)]
        public uint CollectionNumbersLimit { get; set; }


        // Rust type "u32"
        [Serialize(1)]
        public uint AccountTokenOwnershipLimit { get; set; }


        // Rust type "u64"
        [Serialize(2)]
        public ulong CollectionsAdminsLimit { get; set; }


        // Rust type "u32"
        [Serialize(3)]
        public uint CustomDataLimit { get; set; }


        // Rust type "u32"
        [Serialize(4)]
        public uint NftSponsorTimeout { get; set; }


        // Rust type "u32"
        [Serialize(5)]
        public uint FungibleSponsorTimeout { get; set; }


        // Rust type "u32"
        [Serialize(6)]
        public uint RefungibleSponsorTimeout { get; set; }



        public ChainLimits() { }
        public ChainLimits(uint @collectionNumbersLimit, uint @accountTokenOwnershipLimit, ulong @collectionsAdminsLimit, uint @customDataLimit, uint @nftSponsorTimeout, uint @fungibleSponsorTimeout, uint @refungibleSponsorTimeout)
        {
            this.CollectionNumbersLimit = @collectionNumbersLimit;
            this.AccountTokenOwnershipLimit = @accountTokenOwnershipLimit;
            this.CollectionsAdminsLimit = @collectionsAdminsLimit;
            this.CustomDataLimit = @customDataLimit;
            this.NftSponsorTimeout = @nftSponsorTimeout;
            this.FungibleSponsorTimeout = @fungibleSponsorTimeout;
            this.RefungibleSponsorTimeout = @refungibleSponsorTimeout;
        }

    }
}