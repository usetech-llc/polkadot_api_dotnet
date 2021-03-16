using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class SetCollectionLimitsCall : IExtrinsicCall
    {
        // Rust type u32
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type CollectionLimits
        [Serialize(1)]
        public CollectionLimits NewLimits { get; set; }



        public SetCollectionLimitsCall() { }
        public SetCollectionLimitsCall(uint @collectionId, CollectionLimits @newLimits)
        {
            this.CollectionId = @collectionId;
            this.NewLimits = @newLimits;
        }

    }
}