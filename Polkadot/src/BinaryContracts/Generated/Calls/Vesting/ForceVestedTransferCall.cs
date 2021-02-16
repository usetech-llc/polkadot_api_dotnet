using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Vesting
{
    public class ForceVestedTransferCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Source { get; set; }


        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(1)]
        public PublicKey Target { get; set; }


        // Rust type VestingInfo<BalanceOf<T>, T::BlockNumber>
        [Serialize(2)]
        public VestingInfo Schedule { get; set; }



        public ForceVestedTransferCall() { }
        public ForceVestedTransferCall(PublicKey @source, PublicKey @target, VestingInfo @schedule)
        {
            this.Source = @source;
            this.Target = @target;
            this.Schedule = @schedule;
        }

    }
}