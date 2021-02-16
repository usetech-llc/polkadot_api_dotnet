using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public class Rejected : IEvent
    {
        // Rust type ProposalIndex
        [Serialize(0)]
        public ProposalIndex EventArgument0 { get; set; }


        // Rust type Balance
        [Serialize(1)]
        public Balance EventArgument1 { get; set; }



        public Rejected() { }
        public Rejected(ProposalIndex @eventArgument0, Balance @eventArgument1)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
        }

    }
}