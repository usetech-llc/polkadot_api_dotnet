using System.Numerics;
using System.Runtime.InteropServices;

namespace Polkadot.DataStructs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AccountData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] _free;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] _reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] _miscFrozen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] _feeFrozen;
        
        public BigInteger Free => new BigInteger(_free);
        public BigInteger Reserved => new BigInteger(_reserved);
        public BigInteger MiscFrozen => new BigInteger(_miscFrozen);
        public BigInteger FeeFrozen => new BigInteger(_feeFrozen);
    }
}