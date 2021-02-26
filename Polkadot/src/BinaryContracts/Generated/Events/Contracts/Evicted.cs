using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    public partial class Evicted : IEvent
    {
        // Rust type AccountId
        [Serialize(0)]
        public PublicKey EventArgument0 { get; set; }


        // Rust type bool
        [Serialize(1)]
        public bool EventArgument1 { get; set; }



        public Evicted() { }
        public Evicted(PublicKey @eventArgument0, bool @eventArgument1)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
        }

    }
}