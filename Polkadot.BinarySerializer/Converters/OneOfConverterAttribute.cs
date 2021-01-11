using System;

namespace Polkadot.BinarySerializer.Converters
{
    public class OneOfConverterAttribute : ConverterAttribute
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
        
        public OneOfConverterAttribute()
        {
            ConverterType = typeof(OneOfConverter);
            Parameters = new object[2];
        }
    }
}