using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class MathLibExTests
    {
        [Fact]
        public void DivisorInt()
        {
            MathLibEx.Divisor(1).Should().Equal(new int[] { 1 });
            MathLibEx.Divisor(1000000007).Should().Equal(new int[] { 1, 1000000007 });
            MathLibEx.Divisor(49).Should().Equal(new int[] { 1, 7, 49 });
            MathLibEx.Divisor(720).Should().Equal(new int[] {
                1, 2, 3, 4, 5, 6, 8, 9, 10,
                12, 15, 16, 18, 20, 24, 30,
                36, 40, 45, 48, 60, 72, 80,
                90, 120, 144, 180, 240, 360, 720
            });
            MathLibEx.Divisor(6480).Should()
                .StartWith(new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 27, 30, 36, 40, 45, 48, 54, 60, 72, 80, 81 })
                .And
                .EndWith(new int[] { 1620, 2160, 3240, 6480 })
                .And
                .HaveCount(50);

            MathLibEx.Divisor(2095133040).Should().HaveCount(1600); //高度合成数

        }
        [Fact]
        public void DivisorLong()
        {
            MathLibEx.Divisor(1L).Should().Equal(new long[] { 1 });
            MathLibEx.Divisor(128100283921).Should().Equal(new long[] {
                1,
                71,
                5041,
                357911,
                25411681,
                1804229351,
                128100283921 });
            MathLibEx.Divisor(132147483703).Should().Equal(new long[] { 1, 132147483703 });
            MathLibEx.Divisor(963761198400).Should().HaveCount(6720); //高度合成数

        }
    }
}
