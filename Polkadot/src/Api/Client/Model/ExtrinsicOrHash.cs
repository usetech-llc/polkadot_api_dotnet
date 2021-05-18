using OneOf;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Modules.Model
{
    public class ExtrinsicOrHash<THash>
    {
        [OneOfConverter]
        public OneOf<THash, byte[]> Value { get; set; }
    }
}