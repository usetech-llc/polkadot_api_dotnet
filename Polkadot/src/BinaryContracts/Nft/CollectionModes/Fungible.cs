using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Nft.CollectionModes
{
    public class Fungible
    {
        [Serialize(0)]
        public byte DecimalPoints { get; set; }
    }
}