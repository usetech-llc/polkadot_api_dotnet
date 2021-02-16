using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common.CreateItemDatas
{
    public class Fungible
    {
        // Rust type CreateFungibleData
        [Serialize(0)]
        public CreateFungibleData Value { get; set; }



        public Fungible() { }
        public Fungible(CreateFungibleData @value)
        {
            this.Value = @value;
        }

    }
}