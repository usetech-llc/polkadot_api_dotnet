using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events.DispatchClassEnum
{
    public class DispatchClass
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Normal, Operational, Mandatory> Value;

        public DispatchClass()
        {
        }

        public DispatchClass(OneOf<Normal, Operational, Mandatory> value)
        {
            Value = value;
        }
    }
}