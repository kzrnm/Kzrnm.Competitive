
using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class 強連結成分分解Tests
    {
        [Fact]
        public void Random()
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
                    ids[f].Should().BeLessThanOrEqualTo(ids[e.To]);
                }

                var gs = g.SccNewGraph().ToGraph().AsArray();
                for (int i = 0; i < gs.Length; i++)
                {
                    gs[i].Value.Should().Equal(ids.Indexed().Where(t => t.Value == i).Select(t => t.Index));
                }
            }
        }

        [Fact]
        public void 重みなしグラフ()
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
            groupNum.Should().Be(6);
            var scc = g.Scc();

            for (int i = 0; i < 2; i++)
            {
                ids.Should().Equal(0, 1, 2, 3, 3, 4, 5, 3);
                scc[0].Should().Equal(0);
                scc[1].Should().Equal(1);
                scc[2].Should().Equal(2);
                scc[3].Should().Equal(3, 4, 7);
                scc[4].Should().Equal(5);
                scc[5].Should().Equal(6);

                (scc, ids) = g.SccGroupsAndIds();
            }

            var newGraph = g.SccNewGraph().ToGraph();
            newGraph[0].Value.Should().Equal(0);
            newGraph[1].Value.Should().Equal(1);
            newGraph[2].Value.Should().Equal(2);
            newGraph[3].Value.Should().Equal(3, 4, 7);
            newGraph[4].Value.Should().Equal(5);
            newGraph[5].Value.Should().Equal(6);

            newGraph[0].Children.Should().Equal(new GraphEdge(1));
            newGraph[1].Children.Should().Equal(new GraphEdge(2));
            newGraph[2].Children.Should().Equal(new GraphEdge(3));
            newGraph[3].Children.Should().Equal(new GraphEdge(4), new GraphEdge(5));
            newGraph[4].Children.Should().Equal(new GraphEdge(5));
            newGraph[5].Children.Should().BeEmpty();

            newGraph = g.SccNewGraph(false).ToGraph();
            newGraph[0].Value.Should().Equal(0);
            newGraph[1].Value.Should().Equal(1);
            newGraph[2].Value.Should().Equal(2);
            newGraph[3].Value.Should().Equal(3, 4, 7);
            newGraph[4].Value.Should().Equal(5);
            newGraph[5].Value.Should().Equal(6);

            newGraph[0].Children.Should().Equal(new GraphEdge(1));
            newGraph[1].Children.Should().Equal(new GraphEdge(0), new GraphEdge(2));
            newGraph[2].Children.Should().Equal(new GraphEdge(1), new GraphEdge(3));
            newGraph[3].Children.Should().Equal(new GraphEdge(2), new GraphEdge(4), new GraphEdge(5));
            newGraph[4].Children.Should().Equal(new GraphEdge(3), new GraphEdge(5));
            newGraph[5].Children.Should().Equal(new GraphEdge(3), new GraphEdge(4));
        }
        [Fact]
        public void 重み付きグラフ()
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
            groupNum.Should().Be(6);
            var scc = g.Scc();

            for (int i = 0; i < 2; i++)
            {
                ids.Should().Equal(0, 1, 2, 3, 3, 4, 5, 3);
                scc[0].Should().Equal(0);
                scc[1].Should().Equal(1);
                scc[2].Should().Equal(2);
                scc[3].Should().Equal(3, 4, 7);
                scc[4].Should().Equal(5);
                scc[5].Should().Equal(6);

                (scc, ids) = g.SccGroupsAndIds();
            }

            var newGraph = g.SccNewGraph().ToGraph();
            newGraph[0].Value.Should().Equal(0);
            newGraph[1].Value.Should().Equal(1);
            newGraph[2].Value.Should().Equal(2);
            newGraph[3].Value.Should().Equal(3, 4, 7);
            newGraph[4].Value.Should().Equal(5);
            newGraph[5].Value.Should().Equal(6);

            newGraph[0].Children.Should().Equal(new GraphEdge(1));
            newGraph[1].Children.Should().Equal(new GraphEdge(2));
            newGraph[2].Children.Should().Equal(new GraphEdge(3));
            newGraph[3].Children.Should().Equal(new GraphEdge(4), new GraphEdge(5));
            newGraph[4].Children.Should().Equal(new GraphEdge(5));
            newGraph[5].Children.Should().BeEmpty();

            newGraph = g.SccNewGraph(false).ToGraph();
            newGraph[0].Value.Should().Equal(0);
            newGraph[1].Value.Should().Equal(1);
            newGraph[2].Value.Should().Equal(2);
            newGraph[3].Value.Should().Equal(3, 4, 7);
            newGraph[4].Value.Should().Equal(5);
            newGraph[5].Value.Should().Equal(6);

            newGraph[0].Children.Should().Equal(new GraphEdge(1));
            newGraph[1].Children.Should().Equal(new GraphEdge(0), new GraphEdge(2));
            newGraph[2].Children.Should().Equal(new GraphEdge(1), new GraphEdge(3));
            newGraph[3].Children.Should().Equal(new GraphEdge(2), new GraphEdge(4), new GraphEdge(5));
            newGraph[4].Children.Should().Equal(new GraphEdge(3), new GraphEdge(5));
            newGraph[5].Children.Should().Equal(new GraphEdge(3), new GraphEdge(4));
        }
    }
}
