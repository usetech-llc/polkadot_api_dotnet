using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class TransferCall : IExtrinsicCall
    {
        // Rust type T::AccountId
        [Serialize(0)]
        public PublicKey Recipient { get; set; }


        // Rust type CollectionId
        [Serialize(1)]
        public uint CollectionId { get; set; }


        // Rust type TokenId
        [Serialize(2)]
        public uint ItemId { get; set; }


        // Rust type u128
        [Serialize(3)]
        [U128Converter]
        public BigInteger Value { get; set; }



        public TransferCall() { }
        public TransferCall(PublicKey @recipient, uint @collectionId, uint @itemId, BigInteger @value)
        {
            this.Recipient = @recipient;
            this.CollectionId = @collectionId;
            this.ItemId = @itemId;
            this.Value = @value;
        }

    }
}