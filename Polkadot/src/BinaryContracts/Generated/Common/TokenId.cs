using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public class TokenId
    {
        // Rust type u32
        [Serialize(0)]
        public uint Value { get; set; }



        public TokenId() { }
        public TokenId(uint @value)
        {
            this.Value = @value;
        }

    }
}