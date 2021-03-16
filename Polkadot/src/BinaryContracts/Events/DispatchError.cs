using OneOf;
using Polkadot.BinaryContracts.Events.DispatchErrorEnum;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events
{
    public class DispatchError
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Other, CannotLookup, BadOrigin, Module> Value;
    }
}