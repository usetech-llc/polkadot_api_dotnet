using Extensions.Data;
using Polkadot.DataStructs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polkadot.Utils
{
    public class Hash
    {
        public static string GetStorageKey(Hasher type, byte[] bytes)
        {
            // byte[] key = new byte[2 * Consts.STORAGE_KEY_BYTELENGTH + 3];
            string key = string.Empty;

            if (type == Hasher.XXHASH)
            {
                var xxhash1 = XXHash.XXH64(bytes, 0, bytes.Length, 0);
                byte[] bytes1 = new byte[] {
                    (byte)(xxhash1 & 0xFF),
                    (byte)((xxhash1 & 0xFF00) >> 8),
                    (byte)((xxhash1 & 0xFF0000) >> 16),
                    (byte)((xxhash1 & 0xFF000000) >> 24),
                    (byte)((xxhash1 & 0xFF00000000) >> 32),
                    (byte)((xxhash1 & 0xFF0000000000) >> 40),
                    (byte)((xxhash1 & 0xFF000000000000) >> 48),
                    (byte)((xxhash1 & 0xFF00000000000000) >> 56)
                };

                var xxhash2 = XXHash.XXH64(bytes, 0, bytes.Length, 1);
                byte[] bytes2 = new byte[] {
                    (byte)(xxhash2 & 0xFF),
                    (byte)((xxhash2 & 0xFF00) >> 8),
                    (byte)((xxhash2 & 0xFF0000) >> 16),
                    (byte)((xxhash2 & 0xFF000000) >> 24),
                    (byte)((xxhash2 & 0xFF00000000) >> 32),
                    (byte)((xxhash2 & 0xFF0000000000) >> 40),
                    (byte)((xxhash2 & 0xFF000000000000) >> 48),
                    (byte)((xxhash2 & 0xFF00000000000000) >> 56)
                };

                foreach (var bt in bytes1)
                {
                    key += bt.ToString("X2");
                }

                foreach (var bt in bytes2)
                {
                    key += bt.ToString("X2");
                }
            }
            else if (type == Hasher.BLAKE2)
            {
                var config = new Blake2Core.Blake2BConfig { OutputSizeInBytes = 16 };
                var hash = Blake2Core.Blake2B.ComputeHash(bytes, 0, bytes.Length, config);

                foreach (var bt in hash)
                {
                    key += bt.ToString("X2");
                }
            }

            return key;
        }
    }
}
