
using AtCoder;

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

    [Test]
    public async Task Get()
    {
        var single = new FenwickTree<int>(10);
        var bit = new IntFenwickTreeRange(10);

        void Add(int l, int r, int x)
        {
            bit.Add(l, r, x);
            for (int i = l; i < r; i++)
                single.Add(i, x);
        }

        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            int r = rnd.Next(10);
            int l = rnd.Next(r);
            Add(l, r, rnd.Next(1, 10));

            for (int i = 0; i < 10; i++) using (Assert.Multiple())
            {
                var v = single.Get(i);
                await bit.Get(i).Should().BeEqualTo(v);
            }
        }
    }

    [Test]
    public async Task ToArray()
    {
        var single = new FenwickTree<int>(10);
        var bit = new IntFenwickTreeRange(10);

        void Add(int l, int r, int x)
        {
            bit.Add(l, r, x);
            for (int i = l; i < r; i++)
                single.Add(i, x);
        }

        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            int r = rnd.Next(10);
            int l = rnd.Next(r);
            Add(l, r, rnd.Next(1, 10));

            await bit.ToArray().Should().BeStrictlyEquivalentTo(single.ToArray());
        }
    }
}