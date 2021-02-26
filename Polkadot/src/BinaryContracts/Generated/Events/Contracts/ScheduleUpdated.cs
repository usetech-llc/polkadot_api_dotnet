using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    public partial class ScheduleUpdated : IEvent
    {
        // Rust type u32
        [Serialize(0)]
        public uint EventArgument0 { get; set; }



        public ScheduleUpdated() { }
        public ScheduleUpdated(uint @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}