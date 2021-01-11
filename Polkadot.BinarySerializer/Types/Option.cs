using OneOf;
using OneOf.Types;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinarySerializer.Types
{
    public class Option<T>
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<None, T> Value { get; set; }
    }
}