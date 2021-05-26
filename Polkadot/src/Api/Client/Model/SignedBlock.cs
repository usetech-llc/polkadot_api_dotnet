using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Model
{
    public class SignedBlock<TBlock>
    {
        // Full block. 
        public TBlock Block { get; set; }
        /// Block justification.
        public Option<Justification> Justification { get; set; }
    }
}