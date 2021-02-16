using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class UnassignCuratorCall : IExtrinsicCall
    {
        // Rust type Compact<ProposalIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger BountyId { get; set; }



        public UnassignCuratorCall() { }
        public UnassignCuratorCall(BigInteger @bountyId)
        {
            this.BountyId = @bountyId;
        }

    }
}