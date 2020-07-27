using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// Code with the specified hash has been stored.
    /// </summary>
    public class CodeStored : IEvent
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] CodeHash;

        public CodeStored()
        {
        }

        public CodeStored(byte[] codeHash)
        {
            CodeHash = codeHash;
        }
    }
}