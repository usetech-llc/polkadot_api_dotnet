using OneOf;
using OneOf.Types;
using Polkadot.BinaryContracts.Events.DispatchClass;
using Polkadot.BinaryContracts.Events.Pays;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.BinaryContracts.Events
{
    public class DispatchInfo
    {
        [Serialize(0)]
        public ulong Weight;
        
        [Serialize(1)]
        [Converter(ConverterType = typeof(OneOfEnumConverter))]
        public OneOf<Normal, Operational, Mandatory> DispatchClass;

        [Serialize(2)]
        [Converter(ConverterType = typeof(OneOfEnumConverter))]
        public OneOf<YesPay, NoPay> Pays;

        public DispatchInfo()
        {
        }
    }
}