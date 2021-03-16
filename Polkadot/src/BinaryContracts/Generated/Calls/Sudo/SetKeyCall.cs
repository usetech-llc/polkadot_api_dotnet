using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Sudo
{
    public partial class SetKeyCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey New { get; set; }



        public SetKeyCall() { }
        public SetKeyCall(PublicKey @new)
        {
            this.New = @new;
        }

    }
}