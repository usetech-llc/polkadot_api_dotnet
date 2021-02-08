using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class AcceptCuratorCall : IExtrinsicCall
    {
        // Rust type Compact<ProposalIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger BountyId { get; set; }



        public AcceptCuratorCall() { }
        public AcceptCuratorCall(BigInteger @bountyId)
        {
            this.BountyId = @bountyId;
        }

    }
}