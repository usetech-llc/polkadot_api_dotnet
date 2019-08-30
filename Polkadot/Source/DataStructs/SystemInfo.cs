namespace Polkadot.Data
{
    /// <summary>
    /// Responce struct for systemInfo
    /// </summary>
    public struct SystemInfo
    {
        public string ChainId { get; set; }
        public string ChainName { get; set; }
        public string Version { get; set; }
        public int TokenDecimals { get; set; }
        public string TokenSymbol { get; set; }
    }
}