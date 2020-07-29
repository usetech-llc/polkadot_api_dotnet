using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events.BalanceStatusEnum
{
    public class BalanceStatus
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Free, Reserved> Value;

        public BalanceStatus()
        {
        }

        public BalanceStatus(OneOf<Free, Reserved> value)
        {
            Value = value;
        }
    }
}