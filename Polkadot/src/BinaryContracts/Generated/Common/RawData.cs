using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class RawData
    {
        // Rust type Vec<u8>
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] Value { get; set; }



        public RawData() { }
        public RawData(byte[] @value)
        {
            this.Value = @value;
        }

    }
}