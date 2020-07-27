using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Events.Sudo
{
    /// <summary>
    /// A sudo just took place.
    /// </summary>
    public class SudoAsDone : IEvent
    {
        [Serialize(0)]
        public bool Value;

        public SudoAsDone()
        {
        }

        public SudoAsDone(bool value)
        {
            Value = value;
        }
    }
}