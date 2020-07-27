namespace Polkadot.BinarySerializer.Converters
{
    public class FixedSizeArrayConverterAttribute : BaseArrayConverterAttribute
    {
        public int Size
        {
            get => (int) Parameters[2];
            set => Parameters[2] = value;
        }
        
        public FixedSizeArrayConverterAttribute(int size)
        {
            Parameters = new object[3];
            ConverterType = typeof(FixedSizeArrayConverter);
            Size = size;
        }
    }
}