using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Sudo
{
    /// <summary>
    /// The sudoer just switched identity; the old key is supplied.
    /// </summary>
    public class KeyChanged : IEvent
    {
        [Serialize(0)]
        public PublicKey OldKey;

        public KeyChanged()
        {
        }

        public KeyChanged(PublicKey oldKey)
        {
            OldKey = oldKey;
        }
    }
}