using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Grandpa
{
    public class NewAuthorities : IEvent
    {
        // Rust type AuthorityList
        [Serialize(0)]
        public AuthorityList EventArgument0 { get; set; }



        public NewAuthorities() { }
        public NewAuthorities(AuthorityList @eventArgument0)
        {
            this.EventArgument0 = @eventArgument0;
        }

    }
}