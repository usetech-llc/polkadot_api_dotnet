using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Events
{
    public class ExtrinsicSuccess : IEvent
    {
        [Serialize(0)]
        public DispatchInfo DispatchInfo;

        public ExtrinsicSuccess()
        {
        }

        public ExtrinsicSuccess(DispatchInfo dispatchInfo)
        {
            DispatchInfo = dispatchInfo;
        }
    }
}