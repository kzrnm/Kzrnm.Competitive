using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AtCoderLib.整数
{
    public class 約数Tests
    {
        [Fact]
        public void 約数Int()
        {
            約数.GetYakusu(1).Should().Equal(new int[] { 1 });
            約数.GetYakusu(1000000007).Should().Equal(new int[] { 1, 1000000007 });
            約数.GetYakusu(49).Should().Equal(new int[] { 1, 7, 49 });
            約数.GetYakusu(720).Should().Equal(new int[] {
                1, 2, 3, 4, 5, 6, 8, 9, 10,
                12, 15, 16, 18, 20, 24, 30,
                36, 40, 45, 48, 60, 72, 80,
                90, 120, 144, 180, 240, 360, 720
            });
            約数.GetYakusu(6480).Should()
                .StartWith(new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 27, 30, 36, 40, 45, 48, 54, 60, 72, 80, 81 })
                .And
                .EndWith(new int[] { 1620, 2160, 3240, 6480 })
                .And
                .HaveCount(50);

            約数.GetYakusu(2095133040).Should().HaveCount(1600); //高度合成数

        }
        [Fact]
        public void 約数Long()
        {
            約数.GetYakusu(1L).Should().Equal(new long[] { 1 });
            約数.GetYakusu(128100283921).Should().Equal(new long[] {
                1,
                71,
                5041,
                357911,
                25411681,
                1804229351,
                128100283921 });
            約数.GetYakusu(132147483703).Should().Equal(new long[] { 1, 132147483703 });
            約数.GetYakusu(963761198400).Should().HaveCount(6720); //高度合成数

        }
    }
}
