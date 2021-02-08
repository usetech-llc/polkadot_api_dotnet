using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class ApproveCall : IExtrinsicCall
    {
        // Rust type T::AccountId
        [Serialize(0)]
        public PublicKey Spender { get; set; }


        // Rust type CollectionId
        [Serialize(1)]
        public uint CollectionId { get; set; }


        // Rust type TokenId
        [Serialize(2)]
        public uint ItemId { get; set; }


        // Rust type u128
        [Serialize(3)]
        [U128Converter]
        public BigInteger Amount { get; set; }



        public ApproveCall() { }
        public ApproveCall(PublicKey @spender, uint @collectionId, uint @itemId, BigInteger @amount)
        {
            this.Spender = @spender;
            this.CollectionId = @collectionId;
            this.ItemId = @itemId;
            this.Amount = @amount;
        }

    }
}