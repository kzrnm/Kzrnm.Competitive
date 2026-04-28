
namespace Kzrnm.Competitive.Testing.Extensions;

public class MathExtensionTests
{
    [Test]
    [Arguments(1, 0, 1)]
    [Arguments(1, 100, 1)]
    [Arguments(2, 0, 1)]
    [Arguments(2, 10, 1024)]
    [Arguments(2, 62, 4611686018427387904L)]
    [Arguments(4, 5, 1024)]
    [Arguments(17, 4, 83521)]
    [Arguments(17, 0, 1)]
    public async Task Pow(long x, int y, long expected)
    {
        await x.Pow(y).Should().BeEqualTo(expected);
    }
}