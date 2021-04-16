using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class RemoveFromWhiteListCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey Address { get; set; }



        public RemoveFromWhiteListCall() { }
        public RemoveFromWhiteListCall(uint @collectionId, PublicKey @address)
        {
            this.CollectionId = @collectionId;
            this.Address = @address;
        }

    }
}