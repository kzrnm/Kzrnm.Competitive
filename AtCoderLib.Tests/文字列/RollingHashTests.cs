using FluentAssertions;
using Xunit;

namespace AtCoderLib.文字列
{
    public class RollingHashTests
    {
        const string aToM = "abcdefghijklm";
        readonly RollingHash rh;
        public RollingHashTests()
        {
            rh = new RollingHash(aToM + aToM);
        }

        [Fact]
        public void HashMatch()
        {
            for (int l = 0; l < aToM.Length; l++)
                for (int r = l + 1; r <= aToM.Length; r++)
                    rh[l..r].Should().Be(rh[(aToM.Length + l)..(aToM.Length + r)]);
        }
        [Fact]
        public void HashNotMatch()
        {
            var all = 0;
            var dCnt = 0;
            var hashNotMatchCnt = 0;
            for (int l1 = 0; l1 < (aToM.Length * 2); l1++)
                for (int r1 = l1 + 1; r1 <= (aToM.Length * 2); r1++)
                    for (int l2 = 0; l2 < (aToM.Length * 2); l2++)
                        for (int r2 = l2 + 1; r2 <= (aToM.Length * 2); r2++)
                        {
                            ++all;
                            if (!(l1 == l2 && r1 == r2) && !(l1 + aToM.Length == l2 && r1 + aToM.Length == r2))
                            {
                                ++dCnt;
                                if (rh[l1..r1] != rh[l2..r2])
                                    ++hashNotMatchCnt;
                            }
                            else rh[l1..r1].Should().Be(rh[l2..r2]);
                        }
            all.Should().Be((aToM.Length * 2 + 1) * (aToM.Length * 2) / 2 * (aToM.Length * 2 + 1) * (aToM.Length * 2) / 2);
            dCnt.Should().Be(all - (aToM.Length * 2 + 1) * (aToM.Length * 2) / 2 - (aToM.Length + 1) * aToM.Length / 2);
            ((double)hashNotMatchCnt / dCnt).Should().BeGreaterThan(0.99);
        }
    }
}
