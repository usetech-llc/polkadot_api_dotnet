using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class KillStorage : IExtrinsicCall
    {
        [Serialize(0)]
        public Key[] Keys { get; set; }
    }
}