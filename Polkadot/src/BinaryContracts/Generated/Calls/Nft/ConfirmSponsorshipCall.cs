using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class ConfirmSponsorshipCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }



        public ConfirmSponsorshipCall() { }
        public ConfirmSponsorshipCall(uint @collectionId)
        {
            this.CollectionId = @collectionId;
        }

    }
}