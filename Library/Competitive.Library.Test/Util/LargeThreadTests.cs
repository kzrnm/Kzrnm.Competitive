
namespace Kzrnm.Competitive.Testing.Util;

public class LargeThreadTests
{
    [Test, MultipleAssertions]
    public async Task LargeStack()
    {
        await LargeThread.LargeStack(() => F(80000)).Should().BeEqualTo(80000L * 80001L / 2);
    }
    static long F(long x) => x + (x > 0 ? F(x - 1) : 0);
}