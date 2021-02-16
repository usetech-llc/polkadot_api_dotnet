using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public class BountyAwarded : IEvent
    {
        // Rust type BountyIndex
        [Serialize(0)]
        public BountyIndex EventArgument0 { get; set; }


        // Rust type AccountId
        [Serialize(1)]
        public PublicKey EventArgument1 { get; set; }



        public BountyAwarded() { }
        public BountyAwarded(BountyIndex @eventArgument0, PublicKey @eventArgument1)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
        }

    }
}