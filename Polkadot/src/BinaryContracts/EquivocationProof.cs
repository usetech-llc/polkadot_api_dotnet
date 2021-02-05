using Polkadot.BinaryContracts.FinalityGrandpa;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class EquivocationProof
    {
        [Serialize(0)]
        public ulong SetId { get; set; }
        
        [Serialize(1)]
        public Equivocation Equivocation { get; set; }
    }
}