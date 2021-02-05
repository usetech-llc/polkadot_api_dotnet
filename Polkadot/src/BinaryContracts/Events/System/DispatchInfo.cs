using Polkadot.BinaryContracts.Events.DispatchClassEnum;
using Polkadot.BinaryContracts.Events.PaysEnum;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Events.System
{
    public class DispatchInfo
    {
        [Serialize(0)]
        public ulong Weight;
        
        [Serialize(1)]
        public DispatchClass DispatchClass;

        [Serialize(2)]
        public Pays Pays;

        public DispatchInfo()
        {
        }
    }
}