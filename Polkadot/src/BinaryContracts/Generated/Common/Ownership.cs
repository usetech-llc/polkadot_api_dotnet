using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class Ownership
    {
        // Rust type "AccountId"
        [Serialize(0)]
        public PublicKey Owner { get; set; }


        // Rust type "u128"
        [Serialize(1)]
        [U128Converter]
        public BigInteger Fraction { get; set; }



        public Ownership() { }
        public Ownership(PublicKey @owner, BigInteger @fraction)
        {
            this.Owner = @owner;
            this.Fraction = @fraction;
        }

    }
}