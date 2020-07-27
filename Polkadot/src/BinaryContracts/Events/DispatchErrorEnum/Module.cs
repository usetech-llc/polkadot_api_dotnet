using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Events.DispatchErrorEnum
{
    public class Module
    {
        [Serialize(0)]
        public byte Index;
        [Serialize(1)]
        public byte Error;

        public Module()
        {
        }

        public Module(byte index, byte error)
        {
            Index = index;
            Error = error;
        }
    }
}