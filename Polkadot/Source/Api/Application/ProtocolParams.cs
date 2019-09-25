namespace Polkadot.Api
{
    using Polkadot.DataStructs;
    using Polkadot.DataStructs.Metadata;

    internal class ProtocolParameters
    {
        public Hasher FreeBalanceHasher { get; set; }
        public string FreeBalancePrefix { get; set; }
        public int BalanceModuleIndex { get; set; }
        public int TransferMethodIndex { get; set; }
        public byte[] GenesisBlockHash { get; set; }
        public Metadata Metadata { get; set; }
    }
}