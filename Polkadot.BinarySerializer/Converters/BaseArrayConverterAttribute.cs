using System;
using Polkadot.BinarySerializer;

namespace Polkadot.BinarySerializer.Converters
{
    public class BaseArrayConverterAttribute : ConverterAttribute
    {
        public Type ItemConverter
        {
            get => Parameters[0] as Type;
            set => Parameters[0] = value;
        }

        public object[] ItemConverterParameters
        {
            get => Parameters[1] as object[];
            set => Parameters[1] = value;
        }
    }
}