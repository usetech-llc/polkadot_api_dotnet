using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class SetConstOnChainSchemaCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type Vec<u8>
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] Schema { get; set; }



        public SetConstOnChainSchemaCall() { }
        public SetConstOnChainSchemaCall(uint @collectionId, byte[] @schema)
        {
            this.CollectionId = @collectionId;
            this.Schema = @schema;
        }

    }
}