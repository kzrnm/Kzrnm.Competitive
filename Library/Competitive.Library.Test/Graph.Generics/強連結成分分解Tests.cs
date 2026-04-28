namespace Kzrnm.Competitive.Testing.Graph;

public class 強連結成分分解Tests
{
    [Test, MultipleAssertions]
    public async Task Random()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            var n = rnd.Next(10, 50);
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
            var ids = g.SccIds().ids;
            foreach (var (f, e) in g.Edges)
            {
                await ids[f].Should().BeLessThanOrEqualTo(ids[e.To]);
            }

            var gs = g.SccNewGraph().ToGraph().AsArray();
            for (int i = 0; i < gs.Length; i++)
            {
                await gs[i].Value.Should().BeEquivalentOrderTo(ids.Index().Where(t => t.Item == i).Select(t => t.Index));
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task 重みなしグラフ()
    {
        var gb = new GraphBuilder(8, true);
        gb.Add(0, 1);
        gb.Add(1, 2);
        gb.Add(2, 3);
        gb.Add(3, 4);
        gb.Add(4, 5);
        gb.Add(5, 6);
        gb.Add(4, 7);
        gb.Add(7, 3);
        gb.Add(7, 5);
        gb.Add(4, 6);
        var g = gb.ToGraph();
        var (groupNum, ids) = g.SccIds();
        await groupNum.Should().BeEqualTo(6);
        var scc = g.Scc();

        for (int i = 0; i < 2; i++)
        {
            await ids.Should().BeEquivalentOrderTo([0, 1, 2, 3, 3, 4, 5, 3]);
            await scc[0].Should().BeEquivalentOrderTo([0]);
            await scc[1].Should().BeEquivalentOrderTo([1]);
            await scc[2].Should().BeEquivalentOrderTo([2]);
            await scc[3].Should().BeEquivalentOrderTo([3, 4, 7]);
            await scc[4].Should().BeEquivalentOrderTo([5]);
            await scc[5].Should().BeEquivalentOrderTo([6]);

            (scc, ids) = g.SccGroupsAndIds();
        }

        var newGraph = g.SccNewGraph().ToGraph();
        await newGraph[0].Value.Should().BeEquivalentOrderTo([0]);
        await newGraph[1].Value.Should().BeEquivalentOrderTo([1]);
        await newGraph[2].Value.Should().BeEquivalentOrderTo([2]);
        await newGraph[3].Value.Should().BeEquivalentOrderTo([3, 4, 7]);
        await newGraph[4].Value.Should().BeEquivalentOrderTo([5]);
        await newGraph[5].Value.Should().BeEquivalentOrderTo([6]);

        await newGraph[0].Children.Should().BeEquivalentOrderTo([new GraphEdge(1)]);
        await newGraph[1].Children.Should().BeEquivalentOrderTo([new GraphEdge(2)]);
        await newGraph[2].Children.Should().BeEquivalentOrderTo([new GraphEdge(3)]);
        await newGraph[3].Children.Should().BeEquivalentOrderTo([new GraphEdge(4), new GraphEdge(5)]);
        await newGraph[4].Children.Should().BeEquivalentOrderTo([new GraphEdge(5)]);
        await newGraph[5].Children.Should().BeEmpty();

        newGraph = g.SccNewGraph(false).ToGraph();
        await newGraph[0].Value.Should().BeEquivalentOrderTo([0]);
        await newGraph[1].Value.Should().BeEquivalentOrderTo([1]);
        await newGraph[2].Value.Should().BeEquivalentOrderTo([2]);
        await newGraph[3].Value.Should().BeEquivalentOrderTo([3, 4, 7]);
        await newGraph[4].Value.Should().BeEquivalentOrderTo([5]);
        await newGraph[5].Value.Should().BeEquivalentOrderTo([6]);

        await newGraph[0].Children.Should().BeEquivalentOrderTo([new GraphEdge(1)]);
        await newGraph[1].Children.Should().BeEquivalentOrderTo([new GraphEdge(0), new GraphEdge(2)]);
        await newGraph[2].Children.Should().BeEquivalentOrderTo([new GraphEdge(1), new GraphEdge(3)]);
        await newGraph[3].Children.Should().BeEquivalentOrderTo([new GraphEdge(2), new GraphEdge(4), new GraphEdge(5)]);
        await newGraph[4].Children.Should().BeEquivalentOrderTo([new GraphEdge(3), new GraphEdge(5)]);
        await newGraph[5].Children.Should().BeEquivalentOrderTo([new GraphEdge(3), new GraphEdge(4)]);
    }
    [Test, MultipleAssertions]
    public async Task 重み付きグラフ()
    {
        var gb = new WIntGraphBuilder(8, true);
        gb.Add(0, 1, 1);
        gb.Add(1, 2, 2);
        gb.Add(2, 3, 3);
        gb.Add(3, 4, 4);
        gb.Add(4, 5, 5);
        gb.Add(5, 6, 6);
        gb.Add(4, 7, 7);
        gb.Add(7, 3, 8);
        gb.Add(7, 5, 5);
        gb.Add(4, 6, 2);
        var g = gb.ToGraph();
        var (groupNum, ids) = g.SccIds();
        await groupNum.Should().BeEqualTo(6);
        var scc = g.Scc();

        for (int i = 0; i < 2; i++)
        {
            await ids.Should().BeEquivalentOrderTo([0, 1, 2, 3, 3, 4, 5, 3]);
            await scc[0].Should().BeEquivalentOrderTo([0]);
            await scc[1].Should().BeEquivalentOrderTo([1]);
            await scc[2].Should().BeEquivalentOrderTo([2]);
            await scc[3].Should().BeEquivalentOrderTo([3, 4, 7]);
            await scc[4].Should().BeEquivalentOrderTo([5]);
            await scc[5].Should().BeEquivalentOrderTo([6]);

            (scc, ids) = g.SccGroupsAndIds();
        }

        var newGraph = g.SccNewGraph().ToGraph();
        await newGraph[0].Value.Should().BeEquivalentOrderTo([0]);
        await newGraph[1].Value.Should().BeEquivalentOrderTo([1]);
        await newGraph[2].Value.Should().BeEquivalentOrderTo([2]);
        await newGraph[3].Value.Should().BeEquivalentOrderTo([3, 4, 7]);
        await newGraph[4].Value.Should().BeEquivalentOrderTo([5]);
        await newGraph[5].Value.Should().BeEquivalentOrderTo([6]);

        await newGraph[0].Children.Should().BeEquivalentOrderTo([new GraphEdge(1)]);
        await newGraph[1].Children.Should().BeEquivalentOrderTo([new GraphEdge(2)]);
        await newGraph[2].Children.Should().BeEquivalentOrderTo([new GraphEdge(3)]);
        await newGraph[3].Children.Should().BeEquivalentOrderTo([new GraphEdge(4), new GraphEdge(5)]);
        await newGraph[4].Children.Should().BeEquivalentOrderTo([new GraphEdge(5)]);
        await newGraph[5].Children.Should().BeEmpty();

        newGraph = g.SccNewGraph(false).ToGraph();
        await newGraph[0].Value.Should().BeEquivalentOrderTo([0]);
        await newGraph[1].Value.Should().BeEquivalentOrderTo([1]);
        await newGraph[2].Value.Should().BeEquivalentOrderTo([2]);
        await newGraph[3].Value.Should().BeEquivalentOrderTo([3, 4, 7]);
        await newGraph[4].Value.Should().BeEquivalentOrderTo([5]);
        await newGraph[5].Value.Should().BeEquivalentOrderTo([6]);

        await newGraph[0].Children.Should().BeEquivalentOrderTo([new GraphEdge(1)]);
        await newGraph[1].Children.Should().BeEquivalentOrderTo([new GraphEdge(0), new GraphEdge(2)]);
        await newGraph[2].Children.Should().BeEquivalentOrderTo([new GraphEdge(1), new GraphEdge(3)]);
        await newGraph[3].Children.Should().BeEquivalentOrderTo([new GraphEdge(2), new GraphEdge(4), new GraphEdge(5)]);
        await newGraph[4].Children.Should().BeEquivalentOrderTo([new GraphEdge(3), new GraphEdge(5)]);
        await newGraph[5].Children.Should().BeEquivalentOrderTo([new GraphEdge(3), new GraphEdge(4)]);
    }
}