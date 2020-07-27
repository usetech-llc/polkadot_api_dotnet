using OneOf;

namespace Polkadot.BinaryContracts.Events.DispatchErrorEnum
{
    public class DispatchError
    {
        public OneOf<Other, CannotLookup, BadOrigin, Module> Value;
    }
}