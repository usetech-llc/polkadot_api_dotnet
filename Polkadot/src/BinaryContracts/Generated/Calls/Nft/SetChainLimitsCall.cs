using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class SetChainLimitsCall : IExtrinsicCall
    {
        // Rust type ChainLimits
        [Serialize(0)]
        public ChainLimits Limits { get; set; }



        public SetChainLimitsCall() { }
        public SetChainLimitsCall(ChainLimits @limits)
        {
            this.Limits = @limits;
        }

    }
}