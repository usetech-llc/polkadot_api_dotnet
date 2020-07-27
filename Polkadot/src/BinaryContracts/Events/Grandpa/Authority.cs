using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Grandpa
{
    public class Authority
    {
        [Serialize(0)]
        public PublicKey AuthorityId;

        [Serialize(1)]
        public ulong AuthorityWeight;

        public Authority()
        {
        }

        public Authority(PublicKey authorityId, ulong authorityWeight)
        {
            AuthorityId = authorityId;
            AuthorityWeight = authorityWeight;
        }
    }
}