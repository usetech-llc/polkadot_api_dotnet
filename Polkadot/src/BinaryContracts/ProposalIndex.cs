using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class ProposalIndex
    {
        [Serialize(0)]
        public uint Value { get; set; }

        public ProposalIndex()
        {
        }

        public ProposalIndex(uint value)
        {
            Value = value;
        }
    }
}