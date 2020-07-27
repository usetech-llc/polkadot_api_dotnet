using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// Contract deployed by address at the specified address.
    /// </summary>
    public class Instantiated : IEvent
    {
        [Serialize(0)]
        public PublicKey Deployer;
        
        [Serialize(1)]
        public PublicKey Contract;

        public Instantiated()
        {
        }

        public Instantiated(PublicKey deployer, PublicKey contract)
        {
            Deployer = deployer;
            Contract = contract;
        }
    }
}