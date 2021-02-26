using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public partial class Awarded : IEvent
    {
        // Rust type ProposalIndex
        [Serialize(0)]
        public ProposalIndex EventArgument0 { get; set; }


        // Rust type Balance
        [Serialize(1)]
        public Balance EventArgument1 { get; set; }


        // Rust type AccountId
        [Serialize(2)]
        public PublicKey EventArgument2 { get; set; }



        public Awarded() { }
        public Awarded(ProposalIndex @eventArgument0, Balance @eventArgument1, PublicKey @eventArgument2)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
            this.EventArgument2 = @eventArgument2;
        }

    }
}