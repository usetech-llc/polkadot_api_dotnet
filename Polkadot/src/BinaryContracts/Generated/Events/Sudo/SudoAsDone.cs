using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Sudo
{
    public class SudoAsDone : IEvent
    {
        // Rust type bool
        [Serialize(0)]
        public bool EventArgument0 { get; set; }



        public SudoAsDone() { }
        public SudoAsDone(bool @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}