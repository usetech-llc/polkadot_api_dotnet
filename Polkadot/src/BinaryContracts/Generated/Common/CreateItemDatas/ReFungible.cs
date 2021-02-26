using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common.CreateItemDatas
{
    public partial class ReFungible
    {
        // Rust type CreateReFungibleData
        [Serialize(0)]
        public CreateReFungibleData Value { get; set; }



        public ReFungible() { }
        public ReFungible(CreateReFungibleData @value)
        {
            this.Value = @value;
        }

    }
}