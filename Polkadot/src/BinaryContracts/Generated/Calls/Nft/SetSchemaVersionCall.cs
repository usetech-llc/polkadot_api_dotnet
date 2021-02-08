using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class SetSchemaVersionCall : IExtrinsicCall
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint CollectionId { get; set; }


        // Rust type SchemaVersion
        [Serialize(1)]
        public SchemaVersion Version { get; set; }



        public SetSchemaVersionCall() { }
        public SetSchemaVersionCall(uint @collectionId, SchemaVersion @version)
        {
            this.CollectionId = @collectionId;
            this.Version = @version;
        }

    }
}