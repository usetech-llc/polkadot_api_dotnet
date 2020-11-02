using System.IO;
using Polkadot.BinarySerializer;
using Xunit;

namespace PolkaTest
{
    public class ScaleTest
    {
        [Theory]
        [InlineData(0L)]
        [InlineData(62L)]
        [InlineData(63L)]
        [InlineData(64L)]
        [InlineData(65L)]
        [InlineData(4096L)]
        [InlineData(16382L)]
        [InlineData(16383L)]
        [InlineData(16384L)]
        [InlineData(1073741822L)]
        [InlineData(1073741823L)]
        [InlineData(1073741824L)]
        public void EncodingAndDecodingNumbersDoesntChangeThem(long number)
        {
            var encoded = Scale.EncodeCompactInteger(number);
            var ms = new MemoryStream(encoded.Bytes);
            var decoded = Scale.DecodeCompactInteger(ms);
            Assert.Equal(number, (long)decoded.Value);
        }
    }
}