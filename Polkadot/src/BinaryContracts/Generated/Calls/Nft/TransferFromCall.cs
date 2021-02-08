using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class TransferFromCall : IExtrinsicCall
    {
        // Rust type T::AccountId
        [Serialize(0)]
        public PublicKey From { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey Recipient { get; set; }


        // Rust type CollectionId
        [Serialize(2)]
        public uint CollectionId { get; set; }


        // Rust type TokenId
        [Serialize(3)]
        public uint ItemId { get; set; }


        // Rust type u128
        [Serialize(4)]
        [U128Converter]
        public BigInteger Value { get; set; }



        public TransferFromCall() { }
        public TransferFromCall(PublicKey @from, PublicKey @recipient, uint @collectionId, uint @itemId, BigInteger @value)
        {
            this.From = @from;
            this.Recipient = @recipient;
            this.CollectionId = @collectionId;
            this.ItemId = @itemId;
            this.Value = @value;
        }

    }
}