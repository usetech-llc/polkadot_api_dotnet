using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class AccessMode
    {
        // Rust type [          "Normal",          "WhiteList"        ]
        [Serialize(0)]
        [OneOfConverter]
        public OneOf.OneOf<Polkadot.BinaryContracts.Common.AccessModes.Normal, Polkadot.BinaryContracts.Common.AccessModes.WhiteList> Value { get; set; }



        public AccessMode() { }
        public AccessMode(OneOf.OneOf<Polkadot.BinaryContracts.Common.AccessModes.Normal, Polkadot.BinaryContracts.Common.AccessModes.WhiteList> @value)
        {
            this.Value = @value;
        }

    }
}