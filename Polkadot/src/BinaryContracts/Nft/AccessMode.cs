using OneOf;
using Polkadot.BinaryContracts.Nft.AccessModes;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Nft
{
    public class AccessMode
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Normal, WhiteList> Mode { get; set; }
    }
}