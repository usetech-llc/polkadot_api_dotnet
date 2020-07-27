using OneOf;
using Polkadot.BinaryContracts.Events.DispatchErrorEnum;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events.Sudo
{
    /// <summary>
    /// A sudo just took place.
    /// </summary>
    public class Sudid : IEvent
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Empty, DispatchError> Value;

        public Sudid()
        {
        }

        public Sudid(OneOf<Empty, DispatchError> value)
        {
            Value = value;
        }
    }
}