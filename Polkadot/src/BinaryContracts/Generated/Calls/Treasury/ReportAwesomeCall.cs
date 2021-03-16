using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public partial class ReportAwesomeCall : IExtrinsicCall
    {
        // Rust type Vec<u8>
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] Reason { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey Who { get; set; }



        public ReportAwesomeCall() { }
        public ReportAwesomeCall(byte[] @reason, PublicKey @who)
        {
            this.Reason = @reason;
            this.Who = @who;
        }

    }
}