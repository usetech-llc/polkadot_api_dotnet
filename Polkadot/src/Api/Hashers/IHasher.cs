using System.Collections.Generic;

namespace Polkadot.Api.Hashers
{
    public interface IHasher
    {
        byte[] Hash(byte[] key);
        byte[] HashMultiple(IEnumerable<byte[]> keys);
    }
}