using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class KillPrefix : IExtrinsicCall
    {
        [Serialize(0)]
        public Key Prefix { get; set; }
        [Serialize(1)]
        public uint Subkeys { get; set; }
    }
}