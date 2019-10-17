namespace Polkadot.src.DataStructs
{
    using System.Numerics;

    public struct Era
    {
        public BigInteger EraProgress { get; set; }
        public BigInteger EraLength { get; set; }
    }
}
