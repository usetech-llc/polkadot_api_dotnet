using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class SetCollectionSponsorCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey NewSponsor { get; set; }



        public SetCollectionSponsorCall() { }
        public SetCollectionSponsorCall(uint @collectionId, PublicKey @newSponsor)
        {
            this.CollectionId = @collectionId;
            this.NewSponsor = @newSponsor;
        }

    }
}