using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public class CollectionLimits
    {
        // Rust type "u32"
        [Serialize(0)]
        public uint AccountTokenOwnershipLimit { get; set; }


        // Rust type "u32"
        [Serialize(1)]
        public uint SponsoredMintSize { get; set; }


        // Rust type "u32"
        [Serialize(2)]
        public uint TokenLimit { get; set; }


        // Rust type "u32"
        [Serialize(3)]
        public uint SponsorTimeout { get; set; }



        public CollectionLimits() { }
        public CollectionLimits(uint @accountTokenOwnershipLimit, uint @sponsoredMintSize, uint @tokenLimit, uint @sponsorTimeout)
        {
            this.AccountTokenOwnershipLimit = @accountTokenOwnershipLimit;
            this.SponsoredMintSize = @sponsoredMintSize;
            this.TokenLimit = @tokenLimit;
            this.SponsorTimeout = @sponsorTimeout;
        }

    }
}