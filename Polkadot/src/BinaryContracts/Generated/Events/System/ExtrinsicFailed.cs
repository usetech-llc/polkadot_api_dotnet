using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.System
{
    public partial class ExtrinsicFailed : IEvent
    {
        // Rust type DispatchError
        [Serialize(0)]
        public DispatchError EventArgument0 { get; set; }


        // Rust type DispatchInfo
        [Serialize(1)]
        public DispatchInfo EventArgument1 { get; set; }



        public ExtrinsicFailed() { }
        public ExtrinsicFailed(DispatchError @eventArgument0, DispatchInfo @eventArgument1)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
        }

    }
}