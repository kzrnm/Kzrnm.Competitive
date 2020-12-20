using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class ComparerTests
    {
        [Fact]
        public void DefaultReverse()
        {
            ReverseComparer<int>.Default.Compare(2, 1).Should().BeLessThan(0);
            ReverseComparerClass<int>.Default.Compare(2, 1).Should().BeLessThan(0);
            ReverseComparerStruct<int>.Default.Compare(2, 1).Should().BeLessThan(0);
            ReverseComparer<int>.Default.Compare(1, 2).Should().BeGreaterThan(0);
            ReverseComparerClass<int>.Default.Compare(1, 2).Should().BeGreaterThan(0);
            ReverseComparerStruct<int>.Default.Compare(1, 2).Should().BeGreaterThan(0);
            ReverseComparer<int>.Default.Compare(2, 2).Should().Be(0);
            ReverseComparerClass<int>.Default.Compare(2, 2).Should().Be(0);
            ReverseComparerStruct<int>.Default.Compare(2, 2).Should().Be(0);
        }
        [Fact]
        public void ExpressionComparer()
        {
            var cmp = ExComparer<int>.CreateExp(i => -i);
            cmp.Compare(2, 1).Should().BeLessThan(0);
            cmp.Compare(1, 2).Should().BeGreaterThan(0);
            cmp.Compare(2, 2).Should().Be(0);
        }
    }
}