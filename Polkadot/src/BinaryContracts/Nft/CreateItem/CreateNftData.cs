using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Nft.CreateItem
{
    public class CreateNftData
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] ConstData { get; set; }
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] VariableData { get; set; } 
    }
}