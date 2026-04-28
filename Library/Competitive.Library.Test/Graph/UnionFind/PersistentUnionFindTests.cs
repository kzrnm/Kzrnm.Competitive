namespace Kzrnm.Competitive.Testing.Graph;

public class PersistentUnionFindTests
{
    [Test, MultipleAssertions]
    public async Task Simple()
    {
        var ufs = new PersistentUnionFind[2];
        ufs[0] = new PersistentUnionFind(2);
        ufs[1] = ufs[0].Merge(0, 1);
        await ufs[0].Same(0, 1).Should().BeFalse();
        await ufs[1].Same(0, 1).Should().BeTrue();
        await ufs[1].Size(0).Should().BeEqualTo(2);
    }

    [Test, MultipleAssertions]
    public async Task Line()
    {
        int n = 1000;
        var ufs = new PersistentUnionFind[n];
        ufs[0] = new PersistentUnionFind(n);
        for (int i = 0; i + 1 < n; i++)
        {
            ufs[i + 1] = ufs[i].Merge(i, i + 1);
        }
        for (int i = 0; i < n - 1; i++)
        {
            await ufs[i].Size(0).Should().BeEqualTo(i + 1);
            await ufs[i].Size(i).Should().BeEqualTo(i + 1);
        }
        await ufs[^1].Size(0).Should().BeEqualTo(n);
    }

    [Test, MultipleAssertions]
    public async Task LineReverse()
    {
        int n = 1000;
        var ufs = new PersistentUnionFind[n];
        ufs[^1] = new PersistentUnionFind(n);
        for (int i = n - 2; i >= 0; i--)
        {
            ufs[i] = ufs[i + 1].Merge(i, i + 1);
        }
        await ufs[0].Size(0).Should().BeEqualTo(n);
    }
}