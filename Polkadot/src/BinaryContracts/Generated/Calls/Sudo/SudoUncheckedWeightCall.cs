using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Sudo
{
    public class SudoUncheckedWeightCall : IExtrinsicCall
    {
        // Rust type Box<<T as Trait>::Call>
        [Serialize(0)]
        public InheritanceCall<IExtrinsicCall> Call { get; set; }


        // Rust type Weight
        [Serialize(1)]
        public Weight Weight { get; set; }



        public SudoUncheckedWeightCall() { }
        public SudoUncheckedWeightCall(InheritanceCall<IExtrinsicCall> @call, Weight @weight)
        {
            this.Call = @call;
            this.Weight = @weight;
        }

    }
}