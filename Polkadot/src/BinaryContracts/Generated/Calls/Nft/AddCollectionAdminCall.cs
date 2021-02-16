using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class AddCollectionAdminCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey NewAdminId { get; set; }



        public AddCollectionAdminCall() { }
        public AddCollectionAdminCall(uint @collectionId, PublicKey @newAdminId)
        {
            this.CollectionId = @collectionId;
            this.NewAdminId = @newAdminId;
        }

    }
}