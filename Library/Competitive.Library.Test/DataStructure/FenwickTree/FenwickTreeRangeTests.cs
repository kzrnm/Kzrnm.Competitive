
namespace Kzrnm.Competitive.Testing.DataStructure;

public class FenwickTreeRangeTests
{
    [Test, MultipleAssertions]
    public async Task AddAndSum()
    {
        var bit = new IntFenwickTreeRange(10);
        await bit.Sum(10).Should().BeEqualTo(0);
        await bit.Sum(0, 10).Should().BeEqualTo(0);
        await bit[0..10].Should().BeEqualTo(0);

        bit.Add(1, 3, 2);
        await bit.Sum(10).Should().BeEqualTo(4);
        await bit.Sum(0, 10).Should().BeEqualTo(4);
        await bit[0..2].Should().BeEqualTo(2);
        await bit[0..3].Should().BeEqualTo(4);
        await bit[0..10].Should().BeEqualTo(4);

        bit.Add(2, 4, 7);
        await bit.Sum(10).Should().BeEqualTo(18);
        await bit.Sum(0, 10).Should().BeEqualTo(18);
        await bit[0..2].Should().BeEqualTo(2);
        await bit[0..3].Should().BeEqualTo(11);
        await bit[0..10].Should().BeEqualTo(18);
    }
}