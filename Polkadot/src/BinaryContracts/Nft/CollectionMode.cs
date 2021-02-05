using OneOf;
using Polkadot.BinaryContracts.Nft.CollectionModes;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Nft
{
    public class CollectionMode
    {
        [Serialize(0)]
        public OneOf<Invalid, Nft.CollectionModes.Nft, Fungible, ReFungible> Mode { get; set; }
    }
}