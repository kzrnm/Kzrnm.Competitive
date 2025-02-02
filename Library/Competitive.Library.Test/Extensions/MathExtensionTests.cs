
namespace Kzrnm.Competitive.Testing.Extensions;

public class MathExtensionTests
{
    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(1, 100, 1)]
    [InlineData(2, 0, 1)]
    [InlineData(2, 10, 1024)]
    [InlineData(2, 62, 4611686018427387904L)]
    [InlineData(4, 5, 1024)]
    [InlineData(17, 4, 83521)]
    [InlineData(17, 0, 1)]
    public void Pow(long x, int y, long expected)
    {
        x.Pow(y).ShouldBe(expected);
    }
}