using FluentAssertions;
using Xunit;

namespace AtCoder.Util
{
    public class CollectionUtilTests
    {
        [Fact]
        public void Flatten()
        {
            CollectionUtil.Flatten(new[] { "abc", "def", "012", "345", "678" })
                .Should().Equal(
                new char[] { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8' });

            CollectionUtil.Flatten(new[] {
                new[] { 1, 2, 3 },
                new[] { -1, -2, -3 },
                new[] { 4, 5, 6 },
                new[] { -6, -5, -4 },
                new[] { 7, 8, 9 },
            })
                .Should().Equal(
                new[] { 1, 2, 3, -1, -2, -3, 4, 5, 6, -6, -5, -4, 7, 8, 9 });
        }
    }
}
