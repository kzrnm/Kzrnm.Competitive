using FluentAssertions;
using Xunit;

namespace AtCoderLib.Tests.文字列
{
    public class RollingHashTests
    {
        readonly RollingHash rh;
        public RollingHashTests()
        {
            var aToZ = "abcdefghijklmnopqrstuvwxyz";
            rh = new RollingHash(aToZ + aToZ);
        }

        [Fact]
        public void HashMatch()
        {
            for (int l = 0; l < 26; l++)
                for (int r = l + 1; r <= 26; r++)
                    rh[l..r].Should().Be(rh[(26 + l)..(26 + r)]);
        }
        [Fact]
        public void HashNotMatch()
        {
            var all = 0;
            var dCnt = 0;
            var hashNotMatchCnt = 0;
            for (int l1 = 0; l1 < 52; l1++)
                for (int r1 = l1 + 1; r1 <= 52; r1++)
                    for (int l2 = 0; l2 < 52; l2++)
                        for (int r2 = l2 + 1; r2 <= 52; r2++)
                        {
                            ++all;
                            if (!(l1 == l2 && r1 == r2) && !(l1 + 26 == l2 && r1 + 26 == r2))
                            {
                                ++dCnt;
                                if (rh[l1..r1] != rh[l2..r2])
                                    ++hashNotMatchCnt;
                            }
                            else rh[l1..r1].Should().Be(rh[l2..r2]);
                        }
            all.Should().Be(53 * 52 / 2 * 53 * 52 / 2);
            dCnt.Should().Be(all - 53 * 52 / 2 - 27 * 26 / 2);
            ((double)hashNotMatchCnt / dCnt).Should().BeGreaterThan(0.99);
        }
    }
}
