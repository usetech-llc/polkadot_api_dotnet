using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public partial class KillPrefixCall : IExtrinsicCall
    {
        // Rust type Key
        [Serialize(0)]
        public Key Prefix { get; set; }


        // Rust type u32
        [Serialize(1)]
        public uint _subkeys { get; set; }



        public KillPrefixCall() { }
        public KillPrefixCall(Key @prefix, uint @_subkeys)
        {
            this.Prefix = @prefix;
            this._subkeys = @_subkeys;
        }

    }
}