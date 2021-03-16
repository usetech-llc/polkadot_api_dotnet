using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    public partial class Restored : IEvent
    {
        // Rust type AccountId
        [Serialize(0)]
        public PublicKey EventArgument0 { get; set; }


        // Rust type AccountId
        [Serialize(1)]
        public PublicKey EventArgument1 { get; set; }


        // Rust type Hash
        [Serialize(2)]
        public Hash EventArgument2 { get; set; }


        // Rust type Balance
        [Serialize(3)]
        public Balance EventArgument3 { get; set; }



        public Restored() { }
        public Restored(PublicKey @eventArgument0, PublicKey @eventArgument1, Hash @eventArgument2, Balance @eventArgument3)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
            this.EventArgument2 = @eventArgument2;
            this.EventArgument3 = @eventArgument3;
        }

    }
}