using System.Collections.Generic;

namespace Polkadot.Api.Hashers
{
    public class Identity: IHasher
    {
        public byte[] Hash(byte[] key)
        {
            return key;
        }

        public byte[] HashMultiple(IEnumerable<byte[]> keys)
        {
            return Utils.Hash.ConcatThenHash(keys, Hash);
        }
    }
}