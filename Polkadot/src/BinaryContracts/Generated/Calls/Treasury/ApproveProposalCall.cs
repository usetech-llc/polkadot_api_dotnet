using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class ApproveProposalCall : IExtrinsicCall
    {
        // Rust type Compact<ProposalIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger ProposalId { get; set; }



        public ApproveProposalCall() { }
        public ApproveProposalCall(BigInteger @proposalId)
        {
            this.ProposalId = @proposalId;
        }

    }
}