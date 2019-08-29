namespace Polkadot.Source
{
    public struct SystemInfo
    {
        public string chainId { get; set; }
        public string chainName { get; set; }
        public string version { get; set; }
        public int tokenDecimals { get; set; }
        public string tokenSymbol { get; set; }
    }
}