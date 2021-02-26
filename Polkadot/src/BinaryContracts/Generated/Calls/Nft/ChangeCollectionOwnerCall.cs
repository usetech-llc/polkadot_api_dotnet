using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class ChangeCollectionOwnerCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey NewOwner { get; set; }



        public ChangeCollectionOwnerCall() { }
        public ChangeCollectionOwnerCall(uint @collectionId, PublicKey @newOwner)
        {
            this.CollectionId = @collectionId;
            this.NewOwner = @newOwner;
        }

    }
}