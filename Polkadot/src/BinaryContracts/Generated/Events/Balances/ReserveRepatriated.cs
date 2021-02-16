using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;
using Polkadot.BinaryContracts.Events.BalanceStatusEnum;

namespace Polkadot.BinaryContracts.Events.Balances
{
    public class ReserveRepatriated : IEvent
    {
        // Rust type AccountId
        [Serialize(0)]
        public PublicKey EventArgument0 { get; set; }


        // Rust type AccountId
        [Serialize(1)]
        public PublicKey EventArgument1 { get; set; }


        // Rust type Balance
        [Serialize(2)]
        public Balance EventArgument2 { get; set; }


        // Rust type Status
        [Serialize(3)]
        public BalanceStatus EventArgument3 { get; set; }



        public ReserveRepatriated() { }
        public ReserveRepatriated(PublicKey @eventArgument0, PublicKey @eventArgument1, Balance @eventArgument2, BalanceStatus @eventArgument3)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
            this.EventArgument2 = @eventArgument2;
            this.EventArgument3 = @eventArgument3;
        }

    }
}