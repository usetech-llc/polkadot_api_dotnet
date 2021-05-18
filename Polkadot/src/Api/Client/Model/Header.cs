using System.Text.Json.Serialization;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    public class Header<TNumber, THash> 
    {
        /// The parent hash.
        [BinaryJsonConverter]
        public THash ParentHash { get; set; }
        /// The block number.
        [BinaryJsonConverter(BinaryConverterType = typeof(BigEndianUncheckedConverter))]
        public TNumber Number { get; set; }
        /// The state trie merkle root
        [BinaryJsonConverter]
        public THash StateRoot { get; set; }
        /// The merkle root of the extrinsics.
        [BinaryJsonConverter]
        public THash ExtrinsicsRoot { get; set; }
        /// A chain-specific digest of data useful for light clients or referencing auxiliary data.
        public Digest<THash> Digest { get; set; }
    }
}