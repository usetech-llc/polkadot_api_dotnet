using System;
using System.Collections.Generic;

namespace Polkadot.Api.Hashers
{
    public class Blake2_128Concat: IHasher
    {
        public byte[] Hash(byte[] key)
        {
            var config = new Blake2Core.Blake2BConfig { OutputSizeInBits = 128 };
            var hash = new byte[128 / 8 + key.Length];
            var blake = Blake2Core.Blake2B.ComputeHash(key, 0, key.Length, config);
            Array.Copy(blake, 0, hash, 0, blake.Length);
            Array.Copy(key, 0, hash, blake.Length, key.Length);
            return hash;
        }

        public byte[] HashMultiple(IEnumerable<byte[]> keys)
        {
            var config = new Blake2Core.Blake2BConfig { OutputSizeInBits = 128 };
            return Utils.Hash.HashThenConcat(keys, b => Blake2Core.Blake2B.ComputeHash(b, 0, b.Length, config));
        }
    }
}