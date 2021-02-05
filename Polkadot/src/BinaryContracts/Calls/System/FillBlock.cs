using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class FillBlock : IExtrinsicCall
    {
        [Serialize(0)]
        public Perbill Ratio { get; set; }
    }
}