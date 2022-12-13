using FluentAssertions;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class 最小全域木BFSTests
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
            graph.MinimumSpanningTreeBFS().Should().Equal(
                (0, new GraphEdge(1)),
                (0, new GraphEdge(2)),
                (0, new GraphEdge(3)),
                (0, new GraphEdge(4)));
        }
        [Fact]
        public void 重み付きグラフ()
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
            graph.MinimumSpanningTreeBFS().Should().Equal(
                (0, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(2, 10)),
                (0, new WEdge<int>(3, 30)),
                (0, new WEdge<int>(4, 40)));
        }
    }
}
