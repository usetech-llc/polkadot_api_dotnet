namespace Polkadot.src.DataStructs
{
    using System.Numerics;

    public struct SessionOrEpoch
    {
        public bool IsEpoch { get; set; }
        public BigInteger SessionLength { get; set; }
        public BigInteger SessionProgress { get; set; }
        public BigInteger EpochProgress { get; set; }
        public BigInteger EpochLength { get; set; }
    }
}
