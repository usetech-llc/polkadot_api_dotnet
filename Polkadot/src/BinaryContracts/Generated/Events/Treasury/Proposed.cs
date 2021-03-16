using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public partial class Proposed : IEvent
    {
        // Rust type ProposalIndex
        [Serialize(0)]
        public ProposalIndex EventArgument0 { get; set; }



        public Proposed() { }
        public Proposed(ProposalIndex @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}