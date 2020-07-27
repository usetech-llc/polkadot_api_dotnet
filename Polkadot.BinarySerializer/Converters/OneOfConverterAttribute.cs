namespace Polkadot.BinarySerializer.Converters
{
    public class OneOfConverterAttribute : ConverterAttribute
    {
        public OneOfConverterAttribute()
        {
            ConverterType = typeof(OneOfConverter);
        }
    }
}