using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class FungibleItemType
    {
        // Rust type "u128"
        [Serialize(0)]
        [U128Converter]
        public BigInteger Value { get; set; }



        public FungibleItemType() { }
        public FungibleItemType(BigInteger @value)
        {
            this.Value = @value;
        }

    }
}