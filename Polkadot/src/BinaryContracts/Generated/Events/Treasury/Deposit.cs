using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public partial class Deposit : IEvent
    {
        // Rust type Balance
        [Serialize(0)]
        public Balance EventArgument0 { get; set; }



        public Deposit() { }
        public Deposit(Balance @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}