using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    public class Block<THeader, TExtrinsic>
    {
        /// The block header.
        public THeader Header { get; set; }
        /// The accompanying extrinsics.
        [PrefixedArrayConverter]
        public TExtrinsic[] Extrinsics { get; set; }
    }
}