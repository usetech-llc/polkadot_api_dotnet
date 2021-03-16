using Polkadot.BinaryContracts.FinalityGrandpa;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class EquivocationProof<T, U>
    {
        [Serialize(0)]
        public ulong SetId { get; set; }
        
        [Serialize(1)]
        public Equivocation<T, U> Equivocation { get; set; }
    }
}