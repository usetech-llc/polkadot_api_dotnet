using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class ProposeBountyCall : IExtrinsicCall
    {
        // Rust type Compact<BalanceOf<T, I>>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger Value { get; set; }


        // Rust type Vec<u8>
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] Description { get; set; }



        public ProposeBountyCall() { }
        public ProposeBountyCall(BigInteger @value, byte[] @description)
        {
            this.Value = @value;
            this.Description = @description;
        }

    }
}