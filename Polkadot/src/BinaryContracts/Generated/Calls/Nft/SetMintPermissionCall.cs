using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class SetMintPermissionCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type bool
        [Serialize(1)]
        public bool MintPermission { get; set; }



        public SetMintPermissionCall() { }
        public SetMintPermissionCall(uint @collectionId, bool @mintPermission)
        {
            this.CollectionId = @collectionId;
            this.MintPermission = @mintPermission;
        }

    }
}