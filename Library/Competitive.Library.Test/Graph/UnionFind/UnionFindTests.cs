namespace Kzrnm.Competitive.Testing.Graph;

public class UnionFindTests
{
    [Test, MultipleAssertions]
    public async Task Zero()
    {
        var uf = new UnionFind(0);
        await uf.Groups().Should().BeEmpty();
    }

    [Test, MultipleAssertions]
    public async Task Simple()
    {
        var uf = new UnionFind(2);
        await uf.Same(0, 1).Should().BeFalse();
        await uf.Merge(0, 1).Should().BeTrue();
        await uf.Same(0, 1).Should().BeTrue();
        await uf.Size(0).Should().BeEqualTo(2);
    }

    [Test, MultipleAssertions]
    public async Task Line()
    {
        int n = 10000;
        var uf = new UnionFind(n);
        for (int i = 0; i < n - 1; i++)
        {
            uf.Merge(i, i + 1);
        }
        await uf.Size(0).Should().BeEqualTo(n);
        await uf.Groups().Length.Should().BeEqualTo(1);
    }

    [Test, MultipleAssertions]
    public async Task LineReverse()
    {
        int n = 10000;
        var uf = new UnionFind(n);
        for (int i = n - 2; i >= 0; i--)
        {
            uf.Merge(i, i + 1);
        }
        await uf.Size(0).Should().BeEqualTo(n);
        await uf.Groups().Length.Should().BeEqualTo(1);
    }
}