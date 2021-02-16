using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class SetPublicAccessModeCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type AccessMode
        [Serialize(1)]
        public AccessMode Mode { get; set; }



        public SetPublicAccessModeCall() { }
        public SetPublicAccessModeCall(uint @collectionId, AccessMode @mode)
        {
            this.CollectionId = @collectionId;
            this.Mode = @mode;
        }

    }
}