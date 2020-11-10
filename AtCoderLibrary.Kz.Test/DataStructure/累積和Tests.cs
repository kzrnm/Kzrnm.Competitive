using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoder
{
    public class 累積和Tests
    {
        readonly IntSums sums = new IntSums(Enumerable.Range(0, 100).ToArray());

        public static TheoryData Sums_Data = new TheoryData<int, int, int>
        {
            { 0, 100, 4950 },
            { 1, 100, 4950 },
            { 2, 100, 4949 },
            { 1, 99, 4851 },
            { 2, 99, 4850 },
        };

        [Theory]
        [MemberData(nameof(Sums_Data))]
        public void Sums(int from, int toExclusive, int sum)
        {
            sums[from..toExclusive].Should().Be(sum);
            sums[from, toExclusive].Should().Be(sum);
        }
    }
}
