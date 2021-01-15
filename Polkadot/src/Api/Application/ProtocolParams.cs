namespace Polkadot.Api
{
    using Polkadot.DataStructs;
    using Polkadot.DataStructs.Metadata;

    public class ProtocolParameters
    {
        public byte BalanceModuleIndex { get; set; }
        public byte TransferMethodIndex { get; set; }
        public byte[] GenesisBlockHash { get; set; }
        public Metadata Metadata { get; set; }
    }
}