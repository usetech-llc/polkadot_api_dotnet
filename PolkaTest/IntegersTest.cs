using System.Numerics;
using Polkadot.BinarySerializer.Types;
using Xunit;

namespace PolkaTest
{
    public class IntegersTest
    {
        [Theory]
        [InlineData(ulong.MaxValue, ulong.MaxValue, "-1")]
        [InlineData(1UL, 0, "1")]
        public void Int128Tests(ulong l0, ulong l1, string result)
        {
            var int128 = new Int128(l0, l1);
            Assert.Equal(result, int128.Value.ToString());

            var (d0, d1) = int128;
            Assert.Equal(l0, d0);
            Assert.Equal(l1, d1);
        }

        [Theory]
        [InlineData(ulong.MaxValue, ulong.MaxValue, "340282366920938463463374607431768211455")]
        [InlineData(1UL, 0, "1")]
        public void Uint128Tests(ulong l0, ulong l1, string result)
        {
            var uint128 = new UInt128(l0, l1);
            Assert.Equal(result, uint128.Value.ToString());

            var (d0, d1) = uint128;
            Assert.Equal(l0, d0);
            Assert.Equal(l1, d1);
        }

        [Theory]
        [InlineData(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, "-1")]
        [InlineData(1UL, 0, 0, 0, "1")]
        public void Int256Tests(ulong l0, ulong l1, ulong l2, ulong l3, string result)
        {
            var int256 = new Int256(l0, l1, l2, l3);
            Assert.Equal(result, int256.Value.ToString());

            var (d0, d1, d2, d3) = int256;
            Assert.Equal(l0, d0);
            Assert.Equal(l1, d1);
            Assert.Equal(l2, d2);
            Assert.Equal(l3, d3);
        }

        [Theory]
        [InlineData(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, "115792089237316195423570985008687907853269984665640564039457584007913129639935")]
        [InlineData(1UL, 0, 0, 0, "1")]
        public void Uint256Tests(ulong l0, ulong l1, ulong l2, ulong l3, string result)
        {
            var uint256 = new UInt256(l0, l1, l2, l3);
            Assert.Equal(result, uint256.Value.ToString());

            var (d0, d1, d2, d3) = uint256;
            Assert.Equal(l0, d0);
            Assert.Equal(l1, d1);
            Assert.Equal(l2, d2);
            Assert.Equal(l3, d3);
        }

        [Theory]
        [InlineData(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, "-1")]
        [InlineData(1UL, 0, 0, 0, 0, 0, 0, 0, "1")]
        public void Int512Tests(ulong l0, ulong l1, ulong l2, ulong l3, ulong l4, ulong l5, ulong l6, ulong l7, string result)
        {
            var int512 = new Int512(l0, l1, l2, l3, l4, l5, l6, l7);
            Assert.Equal(result, int512.Value.ToString());

            var (d0, d1, d2, d3, d4, d5, d6, d7) = int512;
            Assert.Equal(l0, d0);
            Assert.Equal(l1, d1);
            Assert.Equal(l2, d2);
            Assert.Equal(l3, d3);
            Assert.Equal(l4, d4);
            Assert.Equal(l5, d5);
            Assert.Equal(l6, d6);
            Assert.Equal(l7, d7);
        }

        [Theory]
        [InlineData(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, "13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095")]
        [InlineData(1UL, 0, 0, 0, 0, 0, 0, 0, "1")]
        public void UInt512Tests(ulong l0, ulong l1, ulong l2, ulong l3, ulong l4, ulong l5, ulong l6, ulong l7, string result)
        {
            var int512 = new UInt512(l0, l1, l2, l3, l4, l5, l6, l7);
            Assert.Equal(result, int512.Value.ToString());

            var (d0, d1, d2, d3, d4, d5, d6, d7) = int512;
            Assert.Equal(l0, d0);
            Assert.Equal(l1, d1);
            Assert.Equal(l2, d2);
            Assert.Equal(l3, d3);
            Assert.Equal(l4, d4);
            Assert.Equal(l5, d5);
            Assert.Equal(l6, d6);
            Assert.Equal(l7, d7);
        }
    }
}