using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Graph
{
    public class 最小全域木KruskalTests
    {
        [Fact]
        public void 重みなしグラフ()
        {
            var gb = new GraphBuilder(5, false);
            gb.Add(0, 1);
            gb.Add(0, 2);
            gb.Add(0, 3);
            gb.Add(0, 4);
            gb.Add(1, 2);
            gb.Add(2, 3);
            gb.Add(2, 4);
            gb.Add(4, 3);
            gb.Add(4, 0);
            var graph = gb.ToGraph();
            graph.Kruskal().Should().Equal(
                (0, new Edge(1)),
                (0, new Edge(2)),
                (0, new Edge(3)),
                (0, new Edge(4)));
        }

        [Fact]
        public void 連結ではない()
        {
            var gb = new GraphBuilder(5, false);
            gb.Add(0, 1);
            gb.Add(0, 2);
            gb.Add(1, 2);
            gb.Add(4, 3);
            var graph = gb.ToGraph();
            graph.Kruskal().Should().Equal(
                (0, new Edge(1)),
                (0, new Edge(2)),
                (3, new Edge(4)));
        }

        [Fact]
        public void Int()
        {
            var gb = new WIntGraphBuilder(5, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, 605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, 1);
            var graph = gb.ToGraph();
            graph.Kruskal().Should().Equal(
                (0, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(4, 1)),
                (1, new WEdge<int>(2, 5)),
                (4, new WEdge<int>(3, 6)));
        }

        [Fact]
        public void Long()
        {
            var gb = new WLongGraphBuilder(5, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, 605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, 1);
            var graph = gb.ToGraph();
            graph.Kruskal().Should().Equal(
                (0, new WEdge<long>(1, 1)),
                (0, new WEdge<long>(4, 1)),
                (1, new WEdge<long>(2, 5)),
                (4, new WEdge<long>(3, 6)));
        }
    }
}
