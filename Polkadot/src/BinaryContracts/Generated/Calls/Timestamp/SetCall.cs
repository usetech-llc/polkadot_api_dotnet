using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Timestamp
{
    public partial class SetCall : IExtrinsicCall
    {
        // Rust type Compact<T::Moment>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger Now { get; set; }



        public SetCall() { }
        public SetCall(BigInteger @now)
        {
            this.Now = @now;
        }

    }
}