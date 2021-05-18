using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;
using Polkadot.Api.Client.Model;

namespace Polkadot.BinaryContracts.Events.Treasury
{
    public partial class NewTip : IEvent
    {
        // Rust type Hash
        [Serialize(0)]
        public Hash256 EventArgument0 { get; set; }



        public NewTip() { }
        public NewTip(Hash256 @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}