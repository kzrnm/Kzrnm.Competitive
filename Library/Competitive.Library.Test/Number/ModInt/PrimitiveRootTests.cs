namespace Kzrnm.Competitive.Testing.Number;

[NotInParallel]
public class PrimitiveRootTests
{
    [Test, MultipleAssertions]
    [Arguments(2, 1)]
    [Arguments(3, 2)]
    [Arguments(5, 2)]
    [Arguments(7, 3)]
    [Arguments(11, 2)]
    [Arguments(13, 2)]
    [Arguments(37, 2)]
    [Arguments(211, 2)]
    [Arguments(998244353, 3)]
    [Arguments(1000000007, 5)]
    public async Task Solve(int p, int expected)
    {
        await PrimitiveRoot.Solve(p).Should().BeEqualTo(expected);
        await PrimitiveRoot.Solve((uint)p).Should().BeEqualTo((uint)expected);
    }
}