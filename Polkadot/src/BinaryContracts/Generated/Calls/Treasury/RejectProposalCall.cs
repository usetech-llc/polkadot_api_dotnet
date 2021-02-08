using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class RejectProposalCall : IExtrinsicCall
    {
        // Rust type Compact<ProposalIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger ProposalId { get; set; }



        public RejectProposalCall() { }
        public RejectProposalCall(BigInteger @proposalId)
        {
            this.ProposalId = @proposalId;
        }

    }
}