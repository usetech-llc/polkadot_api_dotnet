using System.Linq;
using Polkadot.Utils;
using Xunit;

namespace PolkaTest
{
    public class ConvertersTest
    {
        [Fact]
        public void ToHexAndBackCorrect()
        {
            var array = Enumerable.Range(0, 256).Select(n => (byte)n).ToArray();

            var encodedArray = array.ToPrefixedHexString();
            var decodedArray = encodedArray.HexToByteArray();
            
            Assert.Equal(array, decodedArray);
        }
    }
}