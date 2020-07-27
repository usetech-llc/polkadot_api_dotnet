using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// Triggered when the current schedule is updated.
    /// </summary>
    public class ScheduleUpdated : IEvent
    {
        [Serialize(0)]
        public uint Value;

        public ScheduleUpdated()
        {
        }

        public ScheduleUpdated(uint value)
        {
            Value = value;
        }
    }
}