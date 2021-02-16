using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Contracts
{
    public class UpdateScheduleCall : IExtrinsicCall
    {
        // Rust type Schedule
        [Serialize(0)]
        public Schedule Schedule { get; set; }



        public UpdateScheduleCall() { }
        public UpdateScheduleCall(Schedule @schedule)
        {
            this.Schedule = @schedule;
        }

    }
}