using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public class Rollover : IEvent
    {
        // Rust type Balance
        [Serialize(0)]
        public Balance EventArgument0 { get; set; }



        public Rollover() { }
        public Rollover(Balance @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}