using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class AwardBountyCall : IExtrinsicCall
    {
        // Rust type Compact<ProposalIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger BountyId { get; set; }


        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(1)]
        public PublicKey Beneficiary { get; set; }



        public AwardBountyCall() { }
        public AwardBountyCall(BigInteger @bountyId, PublicKey @beneficiary)
        {
            this.BountyId = @bountyId;
            this.Beneficiary = @beneficiary;
        }

    }
}