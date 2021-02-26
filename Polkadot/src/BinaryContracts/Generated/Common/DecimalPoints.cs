using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class DecimalPoints
    {
        // Rust type u8
        [Serialize(0)]
        public byte Value { get; set; }



        public DecimalPoints() { }
        public DecimalPoints(byte @value)
        {
            this.Value = @value;
        }

    }
}