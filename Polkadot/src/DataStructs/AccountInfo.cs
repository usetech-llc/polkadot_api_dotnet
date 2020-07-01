using System.Runtime.InteropServices;

namespace Polkadot.DataStructs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AccountInfo
    {
        public uint Nonce;
        public byte RefCount;
        public AccountData AccountData;
    }
}