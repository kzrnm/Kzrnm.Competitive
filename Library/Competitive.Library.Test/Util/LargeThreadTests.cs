
namespace Kzrnm.Competitive.Testing.Util;

public class LargeThreadTests
{
    [Fact]
    public void LargeStack()
    {
        LargeThread.LargeStack(() => F(80000)).ShouldBe(80000L * 80001L / 2);
    }
    static long F(long x) => x + (x > 0 ? F(x - 1) : 0);
}
