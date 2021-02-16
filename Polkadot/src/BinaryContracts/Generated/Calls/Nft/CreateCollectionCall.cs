using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class CreateCollectionCall : IExtrinsicCall
    {
        // Rust type Vec<u16>
        [Serialize(0)]
        [PrefixedArrayConverter]
        public ushort[] CollectionName { get; set; }


        // Rust type Vec<u16>
        [Serialize(1)]
        [PrefixedArrayConverter]
        public ushort[] CollectionDescription { get; set; }


        // Rust type Vec<u8>
        [Serialize(2)]
        [PrefixedArrayConverter]
        public byte[] TokenPrefix { get; set; }


        // Rust type CollectionMode
        [Serialize(3)]
        public CollectionMode Mode { get; set; }



        public CreateCollectionCall() { }
        public CreateCollectionCall(ushort[] @collectionName, ushort[] @collectionDescription, byte[] @tokenPrefix, CollectionMode @mode)
        {
            this.CollectionName = @collectionName;
            this.CollectionDescription = @collectionDescription;
            this.TokenPrefix = @tokenPrefix;
            this.Mode = @mode;
        }

    }
}