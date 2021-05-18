using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;
using Polkadot.Api.Client.Model;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public partial class TipCall : IExtrinsicCall
    {
        // Rust type T::Hash
        [Serialize(0)]
        public Hash256 Hash { get; set; }


        // Rust type Compact<BalanceOf<T, I>>
        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger TipValue { get; set; }



        public TipCall() { }
        public TipCall(Hash256 @hash, BigInteger @tipValue)
        {
            this.Hash = @hash;
            this.TipValue = @tipValue;
        }

    }
}