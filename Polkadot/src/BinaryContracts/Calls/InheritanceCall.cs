using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Calls
{
    public class InheritanceCall<TCall> : IExtrinsicCall where TCall : IExtrinsicCall
    {
        [Serialize(0)]
        [Converter(ConverterType = typeof(CallConverter))]
        public TCall Call { get; set; }

        public InheritanceCall()
        {
        }

        public InheritanceCall(TCall call)
        {
            Call = call;
        }
    }
}