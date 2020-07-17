using System;

namespace Polkadot.BinarySerializer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConverterAttribute : Attribute
    {
        public Type ConverterType;

        public ConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }
    }
}