using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Sudo
{
    public partial class SudoAsDone : IEvent
    {
        // Rust type DispatchResult
        [Serialize(0)]
        public DispatchResult EventArgument0 { get; set; }



        public SudoAsDone() { }
        public SudoAsDone(DispatchResult @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}