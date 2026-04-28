namespace Kzrnm.Competitive.Testing.Graph;

public class UnionFindDataTests
{
    [Test, MultipleAssertions]
    public async Task Zero()
    {
        var uf = new UnionFind<int>([], (a, b) => a + b);
        await uf.Groups().Should().BeEmpty();
    }

    [Test, MultipleAssertions]
    public async Task Simple()
    {
        var uf = new UnionFind<int>([1, 2], (a, b) => a + b);
        await uf.Same(0, 1).Should().BeFalse();

        await uf.Data(0).Should().BeEqualTo(1);
        await uf.Data(1).Should().BeEqualTo(2);

        await uf.Merge(0, 1).Should().BeTrue();
        await uf.Same(0, 1).Should().BeTrue();
        await uf.Size(0).Should().BeEqualTo(2);

        await uf.Data(0).Should().BeEqualTo(3);
        await uf.Data(1).Should().BeEqualTo(3);
    }

    [Test, MultipleAssertions]
    public async Task Line()
    {
        int n = 10000;
        var uf = new UnionFind<long>(Enumerable.Range(0, n).Select(a => (long)a).ToArray(), (a, b) => a + b);
        for (int i = 0; i < n - 1; i++)
        {
            uf.Merge(i, i + 1);
        }
        await uf.Size(0).Should().BeEqualTo(n);
        await uf.Groups().Length.Should().BeEqualTo(1);
        await uf.Data(0).Should().BeEqualTo((long)n * (n - 1) / 2);
    }

    [Test, MultipleAssertions]
    public async Task LineReverse()
    {
        int n = 10000;
        var uf = new UnionFind<long>(Enumerable.Range(0, n).Select(a => (long)a).ToArray(), (a, b) => a + b);
        for (int i = n - 2; i >= 0; i--)
        {
            uf.Merge(i, i + 1);
        }
        await uf.Size(0).Should().BeEqualTo(n);
        await uf.Groups().Length.Should().BeEqualTo(1);
        await uf.Data(0).Should().BeEqualTo((long)n * (n - 1) / 2);
    }
}