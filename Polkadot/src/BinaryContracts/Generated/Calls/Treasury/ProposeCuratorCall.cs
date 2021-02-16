using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class ProposeCuratorCall : IExtrinsicCall
    {
        // Rust type Compact<ProposalIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger BountyId { get; set; }


        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(1)]
        public PublicKey Curator { get; set; }


        // Rust type Compact<BalanceOf<T, I>>
        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger Fee { get; set; }



        public ProposeCuratorCall() { }
        public ProposeCuratorCall(BigInteger @bountyId, PublicKey @curator, BigInteger @fee)
        {
            this.BountyId = @bountyId;
            this.Curator = @curator;
            this.Fee = @fee;
        }

    }
}