using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class BurnItemCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type TokenId
        [Serialize(1)]
        public uint ItemId { get; set; }


        // Rust type u128
        [Serialize(2)]
        [U128Converter]
        public BigInteger Value { get; set; }



        public BurnItemCall() { }
        public BurnItemCall(uint @collectionId, uint @itemId, BigInteger @value)
        {
            this.CollectionId = @collectionId;
            this.ItemId = @itemId;
            this.Value = @value;
        }

    }
}