namespace Polkadot.BinarySerializer.Converters
{
    public class CompactBigIntegerConverterAttribute : ConverterAttribute
    {
        public CompactBigIntegerConverterAttribute()
        {
            ConverterType = typeof(CompactBigIntegerConverter);
        }
    }
}