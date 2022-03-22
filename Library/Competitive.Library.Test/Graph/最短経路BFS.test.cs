using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class 最短経路BFSTests
    {
        [Fact]
        public void 重みなしグラフ()
        {
            var gb = new GraphBuilder(5, true);
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
            graph.ShortestPathBFS(0).Should().Equal(0u, 1u, 1u, 1u, 1u);
            graph.ShortestPathBFS(1).Should().Equal(3u, 0u, 1u, 2u, 2u);
            graph.ShortestPathBFS(2).Should().Equal(2u, 3u, 0u, 1u, 1u);
            graph.ShortestPathBFS(3).Should().Equal(4294967295u, 4294967295u, 4294967295u, 0u, 4294967295u);
            graph.ShortestPathBFS(4).Should().Equal(1u, 2u, 2u, 1u, 0u);

            graph.ShortestPathBFSReverse(0).Should().Equal(0u, 3u, 2u, 4294967295u, 1u);
            graph.ShortestPathBFSReverse(1).Should().Equal(1u, 0u, 3u, 4294967295u, 2u);
            graph.ShortestPathBFSReverse(2).Should().Equal(1u, 1u, 0u, 4294967295u, 2u);
            graph.ShortestPathBFSReverse(3).Should().Equal(1u, 2u, 1u, 0u, 1u);
            graph.ShortestPathBFSReverse(4).Should().Equal(1u, 2u, 1u, 4294967295u, 0u);
        }
        [Fact]
        public void 重み付きグラフ()
        {
            var gb = new WIntGraphBuilder(5, true);
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
            graph.ShortestPathBFS(0).Should().Equal(0u, 1u, 1u, 1u, 1u);
            graph.ShortestPathBFS(1).Should().Equal(3u, 0u, 1u, 2u, 2u);
            graph.ShortestPathBFS(2).Should().Equal(2u, 3u, 0u, 1u, 1u);
            graph.ShortestPathBFS(3).Should().Equal(4294967295u, 4294967295u, 4294967295u, 0u, 4294967295u);
            graph.ShortestPathBFS(4).Should().Equal(1u, 2u, 2u, 1u, 0u);

            graph.ShortestPathBFSReverse(0).Should().Equal(0u, 3u, 2u, 4294967295u, 1u);
            graph.ShortestPathBFSReverse(1).Should().Equal(1u, 0u, 3u, 4294967295u, 2u);
            graph.ShortestPathBFSReverse(2).Should().Equal(1u, 1u, 0u, 4294967295u, 2u);
            graph.ShortestPathBFSReverse(3).Should().Equal(1u, 2u, 1u, 0u, 1u);
            graph.ShortestPathBFSReverse(4).Should().Equal(1u, 2u, 1u, 4294967295u, 0u);
        }
    }
}
