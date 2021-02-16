using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common.CreateItemDatas
{
    public class NFT
    {
        // Rust type CreateNftData
        [Serialize(0)]
        public CreateNftData Value { get; set; }



        public NFT() { }
        public NFT(CreateNftData @value)
        {
            this.Value = @value;
        }

    }
}