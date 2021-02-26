using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class Weight
    {
        // Rust type u64
        [Serialize(0)]
        public ulong Value { get; set; }



        public Weight() { }
        public Weight(ulong @value)
        {
            this.Value = @value;
        }

    }
}