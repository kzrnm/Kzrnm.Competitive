using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoderLib.Tests.文字列
{
    public class ZAlgorithmTests
    {
        [Fact]
        public void MatchAll()
        {
            ZAlgorithmEx.ZAlgorithm(new string('a', 100000)).Should().Equal(Enumerable.Range(0, 100001).Select(i => 100000 - i));
        }
        [Fact]
        public void MatchPart()
        {
            var expected = new int[26001];
            for (int i = 0; i < expected.Length; i++)
                expected[i] = 999 - i % 1000;

            for (int i = 0; i < 1000; i++)
                expected[i] += 1000;
            expected[0] = 26000;
            expected[^1] = 0;
            ZAlgorithmEx.ZAlgorithm(Util.Str).Should().Equal(expected);
        }
    }
}
