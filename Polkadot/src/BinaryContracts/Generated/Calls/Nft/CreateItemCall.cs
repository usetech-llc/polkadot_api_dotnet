using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class CreateItemCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey Owner { get; set; }


        // Rust type CreateItemData
        [Serialize(2)]
        public CreateItemData Data { get; set; }



        public CreateItemCall() { }
        public CreateItemCall(uint @collectionId, PublicKey @owner, CreateItemData @data)
        {
            this.CollectionId = @collectionId;
            this.Owner = @owner;
            this.Data = @data;
        }

    }
}