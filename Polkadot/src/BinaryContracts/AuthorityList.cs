using System;
using Polkadot.BinarySerializer;
using Schnorrkel;

namespace Polkadot.BinaryContracts
{
    public class AuthorityList
    {
        [Serialize(0)]
        public Tuple<PublicKey, ulong> Authorities { get; set; }

        public AuthorityList()
        {
        }

        public AuthorityList(Tuple<PublicKey, ulong> authorities)
        {
            Authorities = authorities;
        }
    }
}