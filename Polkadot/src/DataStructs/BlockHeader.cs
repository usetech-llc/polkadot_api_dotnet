namespace Polkadot.Data
{
    public enum DigestItemKey
    {
        Other,             // 0
        AuthoritiesChange, // 1
        ChangesTrieRoot,   // 2
        SealV0,            // 3
        Consensus,         // 4
        Seal,              // 5
        PreRuntime         // 6
    };

    public struct DigestItem
    {
        public DigestItemKey Key { get; set; }
        public string Value { get; set; }
    };

    public class BlockHeader
    {
        public string ParentHash { get; set; }
        public ulong Number { get; set; }
        public string StateRoot { get; set; }
        public string ExtrinsicsRoot { get; set; }
        public DigestItem[] Digest { get; set; }
    }
}