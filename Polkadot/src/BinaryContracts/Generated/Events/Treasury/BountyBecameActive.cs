using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public partial class BountyBecameActive : IEvent
    {
        // Rust type BountyIndex
        [Serialize(0)]
        public BountyIndex EventArgument0 { get; set; }



        public BountyBecameActive() { }
        public BountyBecameActive(BountyIndex @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}