using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class CloseTipCall : IExtrinsicCall
    {
        // Rust type T::Hash
        [Serialize(0)]
        public Hash Hash { get; set; }



        public CloseTipCall() { }
        public CloseTipCall(Hash @hash)
        {
            this.Hash = @hash;
        }

    }
}