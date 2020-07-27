using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events.Grandpa
{
    /// <summary>
    /// New authority set has been applied.
    /// </summary>
    public class NewAuthorities : IEvent
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public Authority[] Authorities;

        public NewAuthorities()
        {
        }

        public NewAuthorities(Authority[] authorities)
        {
            Authorities = authorities;
        }
    }
}