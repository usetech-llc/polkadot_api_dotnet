using System;

namespace Polkadot.BinarySerializer.Converters
{
    public class TupleConverterAttribute : ConverterAttribute
    {
        public Type[] ItemConverters
        {
            get => Parameters[0] as Type[];
            set => Parameters[0] = value;
        }

        public object[][] ConverterParameters
        {
            get => Parameters[1] as object[][];
            set => Parameters[1] = value;
        }
        
        public TupleConverterAttribute()
        {
            ConverterType = typeof(TupleConverter);
            Parameters = new object[2];
        }
    }
}