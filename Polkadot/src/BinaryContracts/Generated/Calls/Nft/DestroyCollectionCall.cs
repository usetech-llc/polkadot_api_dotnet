using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class DestroyCollectionCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }



        public DestroyCollectionCall() { }
        public DestroyCollectionCall(uint @collectionId)
        {
            this.CollectionId = @collectionId;
        }

    }
}