
namespace Kzrnm.Competitive.Testing.Graph;

public class 最短経路BfsTests
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
        graph.ShortestPathBfs(0).ShouldBe([0u, 1u, 1u, 1u, 1u]);
        graph.ShortestPathBfs(1).ShouldBe([3u, 0u, 1u, 2u, 2u]);
        graph.ShortestPathBfs(2).ShouldBe([2u, 3u, 0u, 1u, 1u]);
        graph.ShortestPathBfs(3).ShouldBe([4294967295u, 4294967295u, 4294967295u, 0u, 4294967295u]);
        graph.ShortestPathBfs(4).ShouldBe([1u, 2u, 2u, 1u, 0u]);

        graph.ShortestPathBfsReverse(0).ShouldBe([0u, 3u, 2u, 4294967295u, 1u]);
        graph.ShortestPathBfsReverse(1).ShouldBe([1u, 0u, 3u, 4294967295u, 2u]);
        graph.ShortestPathBfsReverse(2).ShouldBe([1u, 1u, 0u, 4294967295u, 2u]);
        graph.ShortestPathBfsReverse(3).ShouldBe([1u, 2u, 1u, 0u, 1u]);
        graph.ShortestPathBfsReverse(4).ShouldBe([1u, 2u, 1u, 4294967295u, 0u]);
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
        graph.ShortestPathBfs(0).ShouldBe([0u, 1u, 1u, 1u, 1u]);
        graph.ShortestPathBfs(1).ShouldBe([3u, 0u, 1u, 2u, 2u]);
        graph.ShortestPathBfs(2).ShouldBe([2u, 3u, 0u, 1u, 1u]);
        graph.ShortestPathBfs(3).ShouldBe([4294967295u, 4294967295u, 4294967295u, 0u, 4294967295u]);
        graph.ShortestPathBfs(4).ShouldBe([1u, 2u, 2u, 1u, 0u]);

        graph.ShortestPathBfsReverse(0).ShouldBe([0u, 3u, 2u, 4294967295u, 1u]);
        graph.ShortestPathBfsReverse(1).ShouldBe([1u, 0u, 3u, 4294967295u, 2u]);
        graph.ShortestPathBfsReverse(2).ShouldBe([1u, 1u, 0u, 4294967295u, 2u]);
        graph.ShortestPathBfsReverse(3).ShouldBe([1u, 2u, 1u, 0u, 1u]);
        graph.ShortestPathBfsReverse(4).ShouldBe([1u, 2u, 1u, 4294967295u, 0u]);
    }
}
