using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class KillPrefixCall : IExtrinsicCall
    {
        // Rust type Key
        [Serialize(0)]
        public Key Prefix { get; set; }


        // Rust type u32
        [Serialize(1)]
        public uint Subkeys { get; set; }



        public KillPrefixCall() { }
        public KillPrefixCall(Key @prefix, uint @subkeys)
        {
            this.Prefix = @prefix;
            this.Subkeys = @subkeys;
        }

    }
}