using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class SchemaVersion
    {
        // Rust type [          "ImageURL",          "Unique"        ]
        [Serialize(0)]
        [OneOfConverter]
        public OneOf.OneOf<Polkadot.BinaryContracts.Common.SchemaVersions.ImageURL, Polkadot.BinaryContracts.Common.SchemaVersions.Unique> Value { get; set; }



        public SchemaVersion() { }
        public SchemaVersion(OneOf.OneOf<Polkadot.BinaryContracts.Common.SchemaVersions.ImageURL, Polkadot.BinaryContracts.Common.SchemaVersions.Unique> @value)
        {
            this.Value = @value;
        }

    }
}