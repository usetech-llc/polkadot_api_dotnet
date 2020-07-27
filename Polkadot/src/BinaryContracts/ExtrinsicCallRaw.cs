
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class ExtrinsicCallRaw<TParams> : IExtrinsicCall
    {
        [Serialize(0)]
        public byte ModuleIndex { get; set; }

        [Serialize(1)]
        public byte MethodIndex { get; set; }

        [Serialize(2)]
        public TParams Params { get; set; }

        public ExtrinsicCallRaw()
        {
        }

        public ExtrinsicCallRaw(byte moduleIndex, byte methodIndex, TParams @params)
        {
            ModuleIndex = moduleIndex;
            MethodIndex = methodIndex;
            Params = @params;
        }
    }
}