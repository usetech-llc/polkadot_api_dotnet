using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class Gas
    {
        [Serialize(0)]
        public ulong Value { get; set; }
    }
}