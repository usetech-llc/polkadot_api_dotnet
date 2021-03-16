using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class SafeTransferFromCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type TokenId
        [Serialize(1)]
        public uint ItemId { get; set; }


        // Rust type T::AccountId
        [Serialize(2)]
        public PublicKey NewOwner { get; set; }



        public SafeTransferFromCall() { }
        public SafeTransferFromCall(uint @collectionId, uint @itemId, PublicKey @newOwner)
        {
            this.CollectionId = @collectionId;
            this.ItemId = @itemId;
            this.NewOwner = @newOwner;
        }

    }
}