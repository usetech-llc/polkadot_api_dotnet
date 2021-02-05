using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class Remark : IExtrinsicCall
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] RemarkValue { get; set; }
    }
}