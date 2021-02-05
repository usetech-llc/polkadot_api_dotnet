using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class SetHeapPages : IExtrinsicCall
    {
        [Serialize(0)]
        public ulong Pages { get; set; }
    }
}