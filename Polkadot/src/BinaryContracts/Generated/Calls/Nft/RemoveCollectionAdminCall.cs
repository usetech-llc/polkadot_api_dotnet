using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class RemoveCollectionAdminCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey AccountId { get; set; }



        public RemoveCollectionAdminCall() { }
        public RemoveCollectionAdminCall(uint @collectionId, PublicKey @accountId)
        {
            this.CollectionId = @collectionId;
            this.AccountId = @accountId;
        }

    }
}