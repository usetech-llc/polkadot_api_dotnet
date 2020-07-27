using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events.PhaseEnum
{
    public class Phase
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<ApplyExtrinsic, Finalization, Initialization> Value;

        public Phase()
        {
        }

        public Phase(OneOf<ApplyExtrinsic, Finalization, Initialization> value)
        {
            Value = value;
        }
    }
}