
namespace Kzrnm.Competitive.Testing.DataStructure;

public class FenwickTreeRangeTests
{
    [Fact]
    public void AddAndSum()
    {
        var bit = new IntFenwickTreeRange(10);
        bit.Sum(10).ShouldBe(0);
        bit.Sum(0, 10).ShouldBe(0);
        bit[0..10].ShouldBe(0);

        bit.Add(1, 3, 2);
        bit.Sum(10).ShouldBe(4);
        bit.Sum(0, 10).ShouldBe(4);
        bit[0..2].ShouldBe(2);
        bit[0..3].ShouldBe(4);
        bit[0..10].ShouldBe(4);

        bit.Add(2, 4, 7);
        bit.Sum(10).ShouldBe(18);
        bit.Sum(0, 10).ShouldBe(18);
        bit[0..2].ShouldBe(2);
        bit[0..3].ShouldBe(11);
        bit[0..10].ShouldBe(18);
    }
}