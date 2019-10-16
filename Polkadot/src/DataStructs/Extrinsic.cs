namespace Polkadot.DataStructs
{
    using Polkadot.Api;
    using System.Numerics;

    public class Extrinsic 
    {
        public BigInteger Length { get; set; }
        public Signature Signature { get; set; }
    }
}
