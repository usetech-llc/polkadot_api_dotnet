using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class LookupSource
    {
        // Rust type AccountId
        [Serialize(0)]
        public PublicKey Value { get; set; }



        public LookupSource() { }
        public LookupSource(PublicKey @value)
        {
            this.Value = @value;
        }

    }
}