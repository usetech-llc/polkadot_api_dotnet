using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public partial class FillBlockCall : IExtrinsicCall
    {
        // Rust type Perbill
        [Serialize(0)]
        public Perbill _ratio { get; set; }



        public FillBlockCall() { }
        public FillBlockCall(Perbill @_ratio)
        {
            this._ratio = @_ratio;
        }

    }
}