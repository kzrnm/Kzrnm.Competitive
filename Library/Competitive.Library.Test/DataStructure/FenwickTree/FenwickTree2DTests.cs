
namespace Kzrnm.Competitive.Testing.DataStructure;

public class FenwickTree2DTests
{
    [Test, MultipleAssertions]
    public async Task AddAndQuery()
    {
        var bit = new IntFenwickTree2D(2, 3);
        await bit[0..2][0..3].Should().BeEqualTo(0);
        bit.Add(0, 0, 1);
        bit.Add(0, 1, 2);
        bit.Add(0, 2, 3);
        await bit[0..1][0..3].Should().BeEqualTo(6);
        await bit[0..2][0..3].Should().BeEqualTo(6);
        bit.Add(1, 0, 4);
        bit.Add(1, 1, 5);
        bit.Add(1, 2, 6);
        await bit[0..2][0..1].Should().BeEqualTo(5);
        await bit[1..2][1..2].Should().BeEqualTo(5);
        await bit[0..2][0..2].Should().BeEqualTo(12);
        await bit[0..1][0..3].Should().BeEqualTo(6);
        await bit[1..2][0..3].Should().BeEqualTo(15);
        await bit[0..2][0..3].Should().BeEqualTo(21);
    }
}