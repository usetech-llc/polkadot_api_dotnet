using Polkadot.BinaryContracts.Events;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class DispatchResult
    {
        [Serialize(0)]
        public OneOf.OneOf<Empty, DispatchError> Value { get; set; }
    }
}