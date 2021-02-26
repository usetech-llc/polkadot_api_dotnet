using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class CreateItemData
    {
        // Rust type {          "NFT": "CreateNftData",          "Fungible": "CreateFungibleData",          "ReFungible": "CreateReFungibleData"        }
        [Serialize(0)]
        [OneOfConverter]
        public OneOf.OneOf<Polkadot.BinaryContracts.Common.CreateItemDatas.NFT, Polkadot.BinaryContracts.Common.CreateItemDatas.Fungible, Polkadot.BinaryContracts.Common.CreateItemDatas.ReFungible> Value { get; set; }



        public CreateItemData() { }
        public CreateItemData(OneOf.OneOf<Polkadot.BinaryContracts.Common.CreateItemDatas.NFT, Polkadot.BinaryContracts.Common.CreateItemDatas.Fungible, Polkadot.BinaryContracts.Common.CreateItemDatas.ReFungible> @value)
        {
            this.Value = @value;
        }

    }
}