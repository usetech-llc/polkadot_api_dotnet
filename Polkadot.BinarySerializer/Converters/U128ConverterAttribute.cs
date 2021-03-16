namespace Polkadot.BinarySerializer.Converters
{
    public class U128ConverterAttribute : ConverterAttribute
    {
        public U128ConverterAttribute()
        {
            ConverterType = typeof(U128Converter);
        }
    }
}