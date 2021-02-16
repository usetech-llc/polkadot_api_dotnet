using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
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
        public Weight _weight { get; set; }



        public SudoUncheckedWeightCall() { }
        public SudoUncheckedWeightCall(InheritanceCall<IExtrinsicCall> @call, Weight @_weight)
        {
            this.Call = @call;
            this._weight = @_weight;
        }

    }
}