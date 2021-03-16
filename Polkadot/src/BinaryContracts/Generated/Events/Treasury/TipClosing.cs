using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public partial class TipClosing : IEvent
    {
        // Rust type Hash
        [Serialize(0)]
        public Hash EventArgument0 { get; set; }



        public TipClosing() { }
        public TipClosing(Hash @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}