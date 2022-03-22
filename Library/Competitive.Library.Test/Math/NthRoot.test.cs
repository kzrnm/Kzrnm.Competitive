using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: SAMEAS Library/run.test.py
    public class NthRootTests
    {
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 10000, 1)]
        [InlineData(1, 1000000000, 1)]
        [InlineData(215, 3, 5)]
        [InlineData(216, 3, 6)]
        [InlineData(217, 3, 6)]
        [InlineData(9999999999, 10, 9)]
        [InlineData(10000000000, 10, 10)]
        [InlineData(10000000001, 10, 10)]
        [InlineData(18446744073709551615, 1, 18446744073709551615)]
        [InlineData(18446744073709551615, 2, 4294967295)]
        [InlineData(18446744073709551615, 63, 2)]
        [InlineData(18446744073709551615, 64, 1)]
        [InlineData(1796495231553, 3, 12156)]
        public void IntegerRoot(ulong num, long n, ulong expected)
            => NthRoots.IntegerRoot(num, n).Should().Be(expected);
    }
}
