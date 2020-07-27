namespace Polkadot.Utils
{
    using Polkadot.Api;
    using Polkadot.DataStructs;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class AddressUtils
    {
        public static PublicKey GetPublicKeyFromAddr(string address)
        {
            return GetPublicKeyFromAddr(new Address { Symbols = address });
        }

        public static PublicKey GetPublicKeyFromAddr(Address address)
        {
            var pubkByteList = new List<byte>();

            var bs58decoded = SimpleBase.Base58.Bitcoin.Decode(address.Symbols).ToArray();
            int len = bs58decoded.Length;

            if (len == 35)
            {
                // Check the address checksum
                // Add SS58RPE prefix, remove checksum (2 bytes)
                byte[] ssPrefixed = { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
                pubkByteList.AddRange(ssPrefixed);
                pubkByteList.AddRange(bs58decoded.Take(Consts.PUBLIC_KEY_LENGTH + 1));

                var blake2bHashed = Blake2Core.Blake2B.ComputeHash(pubkByteList.ToArray(), new Blake2Core.Blake2BConfig { OutputSizeInBytes = 64, Key = null });
                if (bs58decoded[Consts.PUBLIC_KEY_LENGTH + 1] != blake2bHashed[0] ||
                    bs58decoded[Consts.PUBLIC_KEY_LENGTH + 2] != blake2bHashed[1])
                {
                    throw new ApplicationException("Address checksum is wrong.");
                }

                return new PublicKey { Bytes = bs58decoded.Skip(1).Take(Consts.PUBLIC_KEY_LENGTH).ToArray() };
            }

            throw new ApplicationException("Address checksum is wrong.");
        }
        
        public static string GetAddrFromPublicKey(PublicKey pubKey)
        {
            var plainAddr = Enumerable
                .Repeat((byte) 0x2A, 1024)
                .ToArray();
            pubKey.Bytes.CopyTo(plainAddr.AsMemory(1));

            // Add control sum
            // Add SS58RPE prefix
            var ssPrefixed = new byte[Consts.SR25519_PUBLIC_SIZE + 8];
            var ssPrefixed1 = new byte[] { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
            ssPrefixed1.CopyTo(ssPrefixed, 0);
            plainAddr.AsSpan(0, Consts.SR25519_PUBLIC_SIZE + 1).CopyTo(ssPrefixed.AsSpan(7));

            //   var blake2bHashed = new byte[1024];

            //  blake2(blake2bHashed, 64, ssPrefixed, SR25519_PUBLIC_SIZE + 8, NULL, 0);
            var blake2bHashed = Blake2Core.Blake2B.ComputeHash(ssPrefixed, 0, Consts.SR25519_PUBLIC_SIZE + 8);
            plainAddr[1 + Consts.PUBLIC_KEY_LENGTH] = blake2bHashed[0];
            plainAddr[2 + Consts.PUBLIC_KEY_LENGTH] = blake2bHashed[1];

            var addrCh = SimpleBase.Base58.Bitcoin.Encode(plainAddr).ToArray();

            // EncodeBase58(plainAddr, SR25519_PUBLIC_SIZE + 3, addrCh);
            //  string result((char*) addrCh);

            return new string(addrCh);
        }
    }
}
