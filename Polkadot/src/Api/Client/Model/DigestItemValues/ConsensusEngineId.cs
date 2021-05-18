using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.DigestItemValues
{
    public class ConsensusEngineId
    {
        [Serialize(0)]
        public uint Value { get; set; }
    }
}