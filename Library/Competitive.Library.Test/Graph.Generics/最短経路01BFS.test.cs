using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class 最短経路01BFSTests
    {
        [Fact]
        public void Int()
        {
            var gb = new WIntGraphBuilder(5, true);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 0);
            gb.Add(0, 3, 1);
            gb.Add(0, 4, 1);
            gb.Add(1, 2, 0);
            gb.Add(2, 3, 1);
            gb.Add(2, 4, 0);
            gb.Add(4, 3, 1);
            gb.Add(4, 0, 1);
            var graph = gb.ToGraph();
            graph.ShortestPath01BFS(0).Should().Equal(new int[] { 0, 1, 0, 1, 0 });
            graph.ShortestPath01BFS(1).Should().Equal(new int[] { 1, 0, 0, 1, 0 });
            graph.ShortestPath01BFS(2).Should().Equal(new int[] { 1, 2, 0, 1, 0 });
            graph.ShortestPath01BFS(3).Should().Equal(new int[] { int.MaxValue, int.MaxValue, int.MaxValue, 0, int.MaxValue });
            graph.ShortestPath01BFS(4).Should().Equal(new int[] { 1, 2, 1, 1, 0 });
        }

        [Fact]
        public void Long()
        {
            var gb = new WLongGraphBuilder(5, true);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 0);
            gb.Add(0, 3, 1);
            gb.Add(0, 4, 1);
            gb.Add(1, 2, 0);
            gb.Add(2, 3, 1);
            gb.Add(2, 4, 0);
            gb.Add(4, 3, 1);
            gb.Add(4, 0, 1);
            var graph = gb.ToGraph();
            graph.ShortestPath01BFS(0).Should().Equal(new long[] { 0, 1, 0, 1, 0 });
            graph.ShortestPath01BFS(1).Should().Equal(new long[] { 1, 0, 0, 1, 0 });
            graph.ShortestPath01BFS(2).Should().Equal(new long[] { 1, 2, 0, 1, 0 });
            graph.ShortestPath01BFS(3).Should().Equal(new long[] { long.MaxValue, long.MaxValue, long.MaxValue, 0, long.MaxValue });
            graph.ShortestPath01BFS(4).Should().Equal(new long[] { 1, 2, 1, 1, 0 });
        }
    }
}
