using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public partial class ClaimBountyCall : IExtrinsicCall
    {
        // Rust type Compact<BountyIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger BountyId { get; set; }



        public ClaimBountyCall() { }
        public ClaimBountyCall(BigInteger @bountyId)
        {
            this.BountyId = @bountyId;
        }

    }
}