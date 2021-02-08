using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Vesting
{
    public class VestedTransferCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Target { get; set; }


        // Rust type VestingInfo<BalanceOf<T>, T::BlockNumber>
        [Serialize(1)]
        public VestingInfo Schedule { get; set; }



        public VestedTransferCall() { }
        public VestedTransferCall(PublicKey @target, VestingInfo @schedule)
        {
            this.Target = @target;
            this.Schedule = @schedule;
        }

    }
}