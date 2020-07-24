namespace Polkadot.BinarySerializer.Converters
{
    public class PrefixedArrayConverterAttribute : BaseArrayConverterAttribute
    {
        public PrefixedArrayConverterAttribute()
        {
            Parameters = new object[2];
            ConverterType = typeof(PrefixedArrayConverter);
        }
    }
}