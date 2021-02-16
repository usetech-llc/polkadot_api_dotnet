using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public class TipClosed : IEvent
    {
        // Rust type Hash
        [Serialize(0)]
        public Hash EventArgument0 { get; set; }


        // Rust type AccountId
        [Serialize(1)]
        public PublicKey EventArgument1 { get; set; }


        // Rust type Balance
        [Serialize(2)]
        public Balance EventArgument2 { get; set; }



        public TipClosed() { }
        public TipClosed(Hash @eventArgument0, PublicKey @eventArgument1, Balance @eventArgument2)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
            this.EventArgument2 = @eventArgument2;
        }

    }
}