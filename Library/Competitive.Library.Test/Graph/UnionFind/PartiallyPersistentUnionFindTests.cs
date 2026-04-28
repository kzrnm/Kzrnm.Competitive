namespace Kzrnm.Competitive.Testing.Graph;

public class PartiallyPersistentUnionFindTests
{
    [Test, MultipleAssertions]
    public async Task Simple()
    {
        var uf = new PartiallyPersistentUnionFind(2);
        await uf.Merge(0, 1).Should().BeTrue();
        await uf.Same(0, 1, 0).Should().BeFalse();
        await uf.Same(0, 1, 1).Should().BeTrue();
        await uf.Size(0, 1).Should().BeEqualTo(2);
    }

    [Test, MultipleAssertions]
    public async Task Line()
    {
        int n = 10000;
        var uf = new PartiallyPersistentUnionFind(n);
        for (int i = 0; i < n - 1; i++)
        {
            uf.Merge(i, i + 1);
        }
        for (int i = 0; i < n - 1; i++)
        {
            await uf.Size(0, i).Should().BeEqualTo(i + 1);
            await uf.Size(i, i).Should().BeEqualTo(i + 1);
        }
        await uf.Version.Should().BeEqualTo(n - 1);
        await uf.Size(0, uf.Version).Should().BeEqualTo(n);
    }

    [Test, MultipleAssertions]
    public async Task LineReverse()
    {
        int n = 50000;
        var uf = new PartiallyPersistentUnionFind(n);
        for (int i = n - 2; i >= 0; i--)
        {
            uf.Merge(i, i + 1);
        }
        await uf.Version.Should().BeEqualTo(n - 1);
        await uf.Size(0, uf.Version).Should().BeEqualTo(n);
    }
}