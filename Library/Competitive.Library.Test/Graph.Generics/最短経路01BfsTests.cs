
namespace Kzrnm.Competitive.Testing.Graph
{
    public class 最短経路01BfsTests
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
            graph.ShortestPath01Bfs(0).Should().Equal([0, 1, 0, 1, 0]);
            graph.ShortestPath01Bfs(1).Should().Equal([1, 0, 0, 1, 0]);
            graph.ShortestPath01Bfs(2).Should().Equal([1, 2, 0, 1, 0]);
            graph.ShortestPath01Bfs(3).Should().Equal([int.MaxValue, int.MaxValue, int.MaxValue, 0, int.MaxValue]);
            graph.ShortestPath01Bfs(4).Should().Equal([1, 2, 1, 1, 0]);
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
            graph.ShortestPath01Bfs(0).Should().Equal([0, 1, 0, 1, 0]);
            graph.ShortestPath01Bfs(1).Should().Equal([1, 0, 0, 1, 0]);
            graph.ShortestPath01Bfs(2).Should().Equal([1, 2, 0, 1, 0]);
            graph.ShortestPath01Bfs(3).Should().Equal([long.MaxValue, long.MaxValue, long.MaxValue, 0, long.MaxValue]);
            graph.ShortestPath01Bfs(4).Should().Equal([1, 2, 1, 1, 0]);
        }
    }
}
