using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.System
{
    public partial class ExtrinsicSuccess : IEvent
    {
        // Rust type DispatchInfo
        [Serialize(0)]
        public DispatchInfo EventArgument0 { get; set; }



        public ExtrinsicSuccess() { }
        public ExtrinsicSuccess(DispatchInfo @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}