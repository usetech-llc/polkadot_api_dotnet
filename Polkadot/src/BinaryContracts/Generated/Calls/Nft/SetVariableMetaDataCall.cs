using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class SetVariableMetaDataCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type TokenId
        [Serialize(1)]
        public uint ItemId { get; set; }


        // Rust type Vec<u8>
        [Serialize(2)]
        [PrefixedArrayConverter]
        public byte[] Data { get; set; }



        public SetVariableMetaDataCall() { }
        public SetVariableMetaDataCall(uint @collectionId, uint @itemId, byte[] @data)
        {
            this.CollectionId = @collectionId;
            this.ItemId = @itemId;
            this.Data = @data;
        }

    }
}