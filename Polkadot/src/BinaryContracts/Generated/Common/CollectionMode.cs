using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class CollectionMode
    {
        // Rust type {          "Invalid": null,          "NFT": null,          "Fungible": "DecimalPoints",          "ReFungible": "DecimalPoints"        }
        [Serialize(0)]
        [OneOfConverter]
        public OneOf.OneOf<Polkadot.BinaryContracts.Common.CollectionModes.Invalid, Polkadot.BinaryContracts.Common.CollectionModes.NFT, Polkadot.BinaryContracts.Common.CollectionModes.Fungible, Polkadot.BinaryContracts.Common.CollectionModes.ReFungible> Value { get; set; }



        public CollectionMode() { }
        public CollectionMode(OneOf.OneOf<Polkadot.BinaryContracts.Common.CollectionModes.Invalid, Polkadot.BinaryContracts.Common.CollectionModes.NFT, Polkadot.BinaryContracts.Common.CollectionModes.Fungible, Polkadot.BinaryContracts.Common.CollectionModes.ReFungible> @value)
        {
            this.Value = @value;
        }

    }
}