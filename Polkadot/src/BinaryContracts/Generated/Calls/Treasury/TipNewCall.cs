using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public partial class TipNewCall : IExtrinsicCall
    {
        // Rust type Vec<u8>
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] Reason { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey Who { get; set; }


        // Rust type Compact<BalanceOf<T, I>>
        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger TipValue { get; set; }



        public TipNewCall() { }
        public TipNewCall(byte[] @reason, PublicKey @who, BigInteger @tipValue)
        {
            this.Reason = @reason;
            this.Who = @who;
            this.TipValue = @tipValue;
        }

    }
}