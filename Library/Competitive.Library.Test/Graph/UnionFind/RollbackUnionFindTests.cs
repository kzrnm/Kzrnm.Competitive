namespace Kzrnm.Competitive.Testing.Graph;

public class RollbackUnionFindTests
{
    [Test, MultipleAssertions]
    public async Task Simple()
    {
        var uf = new RollbackUnionFind(8);
        await uf.Same(0, 1).Should().BeFalse();
        await uf.Merge(0, 1).Should().BeTrue();
        await uf.Same(0, 1).Should().BeTrue();
        await uf.Size(0).Should().BeEqualTo(2);

        uf.Undo();
        await uf.Same(0, 1).Should().BeFalse();
        await uf.Size(0).Should().BeEqualTo(1);

        await uf.Merge(0, 1).Should().BeTrue();
        await uf.Merge(0, 2).Should().BeTrue();
        await uf.Size(0).Should().BeEqualTo(3);
        uf.Snapshot();
        await uf.Merge(0, 3).Should().BeTrue();
        await uf.Merge(0, 4).Should().BeTrue();
        var version = uf.Version;
        await uf.Merge(7, 6).Should().BeTrue();
        await uf.Merge(7, 5).Should().BeTrue();
        uf.Merge(0, 7);

        await uf.Size(0).Should().BeEqualTo(8);
        uf.Rollback(version);
        await uf.Size(0).Should().BeEqualTo(5);

        uf.Rollback();
        await uf.Size(0).Should().BeEqualTo(3);
    }

    [Test, MultipleAssertions]
    public async Task Line()
    {
        int n = 10000;
        var uf = new RollbackUnionFind(n);
        for (int i = 0; i < n - 1; i++)
        {
            await uf.Size(0).Should().BeEqualTo(i + 1);
            uf.Merge(i, i + 1);
            await uf.Size(0).Should().BeEqualTo(i + 2);
            await uf.Version.Should().BeEqualTo(i + 1);
        }
        for (int i = n - 2; i >= 0; i--)
        {
            uf.Undo();
            await uf.Size(0).Should().BeEqualTo(i + 1);
            await uf.Version.Should().BeEqualTo(i);
        }
        await uf.Version.Should().BeEqualTo(0);
    }
}