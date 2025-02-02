namespace Kzrnm.Competitive.Testing.Graph;

public class PartiallyPersistentUnionFindTests
{
    [Fact]
    public void Simple()
    {
        var uf = new PartiallyPersistentUnionFind(2);
        uf.Merge(0, 1).ShouldBeTrue();
        uf.Same(0, 1, 0).ShouldBeFalse();
        uf.Same(0, 1, 1).ShouldBeTrue();
        uf.Size(0, 1).ShouldBe(2);
    }

    [Fact]
    public void Line()
    {
        int n = 10000;
        var uf = new PartiallyPersistentUnionFind(n);
        for (int i = 0; i < n - 1; i++)
        {
            uf.Merge(i, i + 1);
        }
        for (int i = 0; i < n - 1; i++)
        {
            uf.Size(0, i).ShouldBe(i + 1);
            uf.Size(i, i).ShouldBe(i + 1);
        }
        uf.Version.ShouldBe(n - 1);
        uf.Size(0, uf.Version).ShouldBe(n);
    }

    [Fact]
    public void LineReverse()
    {
        int n = 50000;
        var uf = new PartiallyPersistentUnionFind(n);
        for (int i = n - 2; i >= 0; i--)
        {
            uf.Merge(i, i + 1);
        }
        uf.Version.ShouldBe(n - 1);
        uf.Size(0, uf.Version).ShouldBe(n);
    }
}
