using OneOf;
using Polkadot.BinaryContracts.Nft.SchemaVersions;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Nft
{
    public class SchemaVersion
    {
        [Serialize(0)]
        public OneOf<ImageUrl, Unique> Version { get; set; }
    }
}