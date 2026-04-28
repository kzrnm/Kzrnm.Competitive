using AtCoder;

namespace Kzrnm.Competitive.Testing.Graph;

public class 行列木定理Tests
{
    [Test, MultipleAssertions]
    public async Task Single()
    {
        var gb = new GraphBuilder(1, false);
        var g = gb.ToGraph();
        await g.MatrixTreeTheorem().Calc<StaticModInt<Mod1000000007>>().Value.Should().BeEqualTo(1);
    }

    [Test, MultipleAssertions]
    public async Task Random()
    {
        var rnd = new Random(227);
        for (int i = 0; i < 100; i++)
        {
            var n = rnd.Next(2, 7);
            var uf = new UnionFind(n);
            var hs = new HashSet<(int u, int v)>();

            while (uf.Groups().Length > 1)
                for (int j = 0; j < n; j++)
                {
                    var (u, v) = rnd.Choice2(0, n);
                    hs.Add((u, v));
                    uf.Merge(u, v);
                }
            var gb = new GraphBuilder(n, false);
            foreach (var (u, v) in hs)
            {
                gb.Add(u, v);
            }

            var g = gb.ToGraph();
            await g.MatrixTreeTheorem().Calc<StaticModInt<Mod1000000007>>().Value.Should().BeEqualTo(Naive(n, hs.ToArray()));
        }
    }

    static int Naive(int n, (int u, int v)[] edges)
    {
        int cnt = 0;
        foreach (var es in IterTools.Combinations(edges, n - 1))
        {
            var uf = new UnionFind(n);
            foreach (var (u, v) in es)
            {
                uf.Merge(u, v);
            }
            if (uf.Groups().Length == 1)
                ++cnt;
        }
        return cnt;
    }
}