using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class RetractTipCall : IExtrinsicCall
    {
        // Rust type T::Hash
        [Serialize(0)]
        public Hash Hash { get; set; }



        public RetractTipCall() { }
        public RetractTipCall(Hash @hash)
        {
            this.Hash = @hash;
        }

    }
}