namespace Kzrnm.Competitive.Testing.Graph;

public class RollbackUnionFindTests
{
    [Fact]
    public void Simple()
    {
        var uf = new RollbackUnionFind(8);
        uf.Same(0, 1).ShouldBeFalse();
        uf.Merge(0, 1).ShouldBeTrue();
        uf.Same(0, 1).ShouldBeTrue();
        uf.Size(0).ShouldBe(2);

        uf.Undo();
        uf.Same(0, 1).ShouldBeFalse();
        uf.Size(0).ShouldBe(1);

        uf.Merge(0, 1).ShouldBeTrue();
        uf.Merge(0, 2).ShouldBeTrue();
        uf.Size(0).ShouldBe(3);
        uf.Snapshot();
        uf.Merge(0, 3).ShouldBeTrue();
        uf.Merge(0, 4).ShouldBeTrue();
        var version = uf.Version;
        uf.Merge(7, 6).ShouldBeTrue();
        uf.Merge(7, 5).ShouldBeTrue();
        uf.Merge(0, 7);

        uf.Size(0).ShouldBe(8);
        uf.Rollback(version);
        uf.Size(0).ShouldBe(5);

        uf.Rollback();
        uf.Size(0).ShouldBe(3);
    }

    [Fact]
    public void Line()
    {
        int n = 10000;
        var uf = new RollbackUnionFind(n);
        for (int i = 0; i < n - 1; i++)
        {
            uf.Size(0).ShouldBe(i + 1);
            uf.Merge(i, i + 1);
            uf.Size(0).ShouldBe(i + 2);
            uf.Version.ShouldBe(i + 1);
        }
        for (int i = n - 2; i >= 0; i--)
        {
            uf.Undo();
            uf.Size(0).ShouldBe(i + 1);
            uf.Version.ShouldBe(i);
        }
        uf.Version.ShouldBe(0);
    }
}
