using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public class BountyExtended : IEvent
    {
        // Rust type BountyIndex
        [Serialize(0)]
        public BountyIndex EventArgument0 { get; set; }



        public BountyExtended() { }
        public BountyExtended(BountyIndex @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}