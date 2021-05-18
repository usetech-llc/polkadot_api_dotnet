namespace Polkadot.BinarySerializer.Converters
{
    public class BigEndianUncheckedConverterAttribute : ConverterAttribute
    {
        public BigEndianUncheckedConverterAttribute()
        {
            ConverterType = typeof(BigEndianUncheckedConverter);
        }
    }
}