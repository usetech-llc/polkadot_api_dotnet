using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events
{
    public class NewAccount : IEvent
    {
        [Serialize(0)]
        public PublicKey Account;

        public NewAccount()
        {
        }

        public NewAccount(PublicKey account)
        {
            Account = account;
        }
    }
}