using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class AddToWhiteListCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey Address { get; set; }



        public AddToWhiteListCall() { }
        public AddToWhiteListCall(uint @collectionId, PublicKey @address)
        {
            this.CollectionId = @collectionId;
            this.Address = @address;
        }

    }
}