using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// An event deposited upon execution of a contract from the account.
    /// </summary>
    public class ContractExecution : IEvent
    {
        [Serialize(0)]
        public PublicKey Account;

        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] Array;

        public ContractExecution()
        {
        }

        public ContractExecution(PublicKey account, byte[] array)
        {
            Account = account;
            Array = array;
        }
    }
}