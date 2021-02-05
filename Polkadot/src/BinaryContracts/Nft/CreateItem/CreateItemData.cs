using OneOf;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Nft.CreateItem
{
    public class CreateItemData
    {
        [Serialize(0)]
        public OneOf<CreateNftData, CreateFungibleData, CreateReFungibleData> Data { get; set; }
    }
}