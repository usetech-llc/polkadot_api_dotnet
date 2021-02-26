using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class SetVariableOnChainSchemaCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type Vec<u8>
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] Schema { get; set; }



        public SetVariableOnChainSchemaCall() { }
        public SetVariableOnChainSchemaCall(uint @collectionId, byte[] @schema)
        {
            this.CollectionId = @collectionId;
            this.Schema = @schema;
        }

    }
}