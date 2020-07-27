using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Events.PhaseEnum
{
    public class ApplyExtrinsic
    {
        [Serialize(0)]
        public uint Value { get; set; }

        public ApplyExtrinsic()
        {
        }

        public ApplyExtrinsic(uint value)
        {
            Value = value;
        }
    }
}