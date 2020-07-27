using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events.PaysEnum
{
    public class Pays
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<YesPay, NoPay> Value;

        public Pays()
        {
        }

        public Pays(OneOf<YesPay, NoPay> value)
        {
            Value = value;
        }
    }
}