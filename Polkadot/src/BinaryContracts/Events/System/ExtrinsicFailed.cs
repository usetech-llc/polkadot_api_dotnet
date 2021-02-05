using Polkadot.BinaryContracts.Events.DispatchErrorEnum;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Events.System
{
    public class ExtrinsicFailed : IEvent
    {
        [Serialize(0)]
        public DispatchError DispatchError;

        [Serialize(1)]
        public DispatchInfo DispatchInfo;

        public ExtrinsicFailed()
        {
        }

        public ExtrinsicFailed(DispatchError dispatchError, DispatchInfo dispatchInfo)
        {
            DispatchError = dispatchError;
            DispatchInfo = dispatchInfo;
        }
    }
}