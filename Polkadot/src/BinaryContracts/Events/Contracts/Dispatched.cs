using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// A call was dispatched from the given account. The bool signals whether it was
    /// successful execution or not.
    /// </summary>
    public class Dispatched : IEvent
    {
        [Serialize(0)]
        public PublicKey Dispatcher;

        [Serialize(1)]
        public bool Success;

        public Dispatched()
        {
        }

        public Dispatched(PublicKey dispatcher, bool success)
        {
            Dispatcher = dispatcher;
            Success = success;
        }
    }
}