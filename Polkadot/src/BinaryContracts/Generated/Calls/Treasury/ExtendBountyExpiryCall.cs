using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public partial class ExtendBountyExpiryCall : IExtrinsicCall
    {
        // Rust type Compact<BountyIndex>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger BountyId { get; set; }


        // Rust type Vec<u8>
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] _remark { get; set; }



        public ExtendBountyExpiryCall() { }
        public ExtendBountyExpiryCall(BigInteger @bountyId, byte[] @_remark)
        {
            this.BountyId = @bountyId;
            this._remark = @_remark;
        }

    }
}