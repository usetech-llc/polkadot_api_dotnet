using System.Collections.Generic;

namespace Polkadot.Api.Hashers
{
    public class Blake2_128: IHasher
    {
        public byte[] Hash(byte[] key)
        {
            var config = new Blake2Core.Blake2BConfig { OutputSizeInBits = 128 };
            return Blake2Core.Blake2B.ComputeHash(key, 0, key.Length, config);
        }

        public byte[] HashMultiple(IEnumerable<byte[]> keys)
        {
            var config = new Blake2Core.Blake2BConfig { OutputSizeInBits = 128 };
            return Utils.Hash.ConcatThenHash(keys, b => Blake2Core.Blake2B.ComputeHash(b, 0, b.Length, config));
        }
    }
}