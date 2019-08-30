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
        ulong Number { get; set; }
        string StateRoot { get; set; }
        string ExtrinsicsRoot { get; set; }
        DigestItem[] Digest { get; set; }
    }
}