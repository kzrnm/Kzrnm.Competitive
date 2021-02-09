using FluentAssertions;
using Xunit;

namespace AtCoder.Graph
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
            graph.ShortestPathBFS(0).Should().Equal(new[] { 0, 1, 1, 1, 1 });
            graph.ShortestPathBFS(1).Should().Equal(new[] { 3, 0, 1, 2, 2 });
            graph.ShortestPathBFS(2).Should().Equal(new[] { 2, 3, 0, 1, 1 });
            graph.ShortestPathBFS(3).Should().Equal(new[] { 2147483647, 2147483647, 2147483647, 0, 2147483647 });
            graph.ShortestPathBFS(4).Should().Equal(new[] { 1, 2, 2, 1, 0 });

            graph.ShortestPathBFSReverse(0).Should().Equal(new[] { 0, 3, 2, 2147483647, 1 });
            graph.ShortestPathBFSReverse(1).Should().Equal(new[] { 1, 0, 3, 2147483647, 2 });
            graph.ShortestPathBFSReverse(2).Should().Equal(new[] { 1, 1, 0, 2147483647, 2 });
            graph.ShortestPathBFSReverse(3).Should().Equal(new[] { 1, 2, 1, 0, 1 });
            graph.ShortestPathBFSReverse(4).Should().Equal(new[] { 1, 2, 1, 2147483647, 0 });
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
            graph.ShortestPathBFS(0).Should().Equal(new[] { 0, 1, 1, 1, 1 });
            graph.ShortestPathBFS(1).Should().Equal(new[] { 3, 0, 1, 2, 2 });
            graph.ShortestPathBFS(2).Should().Equal(new[] { 2, 3, 0, 1, 1 });
            graph.ShortestPathBFS(3).Should().Equal(new[] { 2147483647, 2147483647, 2147483647, 0, 2147483647 });
            graph.ShortestPathBFS(4).Should().Equal(new[] { 1, 2, 2, 1, 0 });

            graph.ShortestPathBFSReverse(0).Should().Equal(new[] { 0, 3, 2, 2147483647, 1 });
            graph.ShortestPathBFSReverse(1).Should().Equal(new[] { 1, 0, 3, 2147483647, 2 });
            graph.ShortestPathBFSReverse(2).Should().Equal(new[] { 1, 1, 0, 2147483647, 2 });
            graph.ShortestPathBFSReverse(3).Should().Equal(new[] { 1, 2, 1, 0, 1 });
            graph.ShortestPathBFSReverse(4).Should().Equal(new[] { 1, 2, 1, 2147483647, 0 });
        }
    }
}
