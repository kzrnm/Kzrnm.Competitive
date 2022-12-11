using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Util
{
    public class LargeThreadTests
    {
        [Fact]
        public void LargeStack()
        {
            LargeThread.LargeStack(() => F(80000)).Should().Be(80000L * 80001L / 2);
        }
        static long F(long x) => x + (x > 0 ? F(x - 1) : 0);
    }
}
