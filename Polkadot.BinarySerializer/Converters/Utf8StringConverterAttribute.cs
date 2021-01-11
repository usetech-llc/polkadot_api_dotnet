namespace Polkadot.BinarySerializer.Converters
{
    public class Utf8StringConverterAttribute : ConverterAttribute
    {
        public Utf8StringConverterAttribute()
        {
            ConverterType = typeof(Utf8StringConverter);
        }
    }
}