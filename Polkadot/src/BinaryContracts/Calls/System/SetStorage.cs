using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class SetStorage : IExtrinsicCall
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public KeyValue[] Items { get; set; }
    }
}