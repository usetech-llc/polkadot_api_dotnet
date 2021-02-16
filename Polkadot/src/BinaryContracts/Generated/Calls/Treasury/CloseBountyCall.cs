using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class CloseBountyCall : IExtrinsicCall
    {
        // Rust type Compact<BountyIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger BountyId { get; set; }



        public CloseBountyCall() { }
        public CloseBountyCall(BigInteger @bountyId)
        {
            this.BountyId = @bountyId;
        }

    }
}