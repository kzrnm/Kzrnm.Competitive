
namespace Kzrnm.Competitive.Testing.Graph;

public class 最短経路BfsTests
{
    [Test, MultipleAssertions]
    public async Task 重みなしグラフ()
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
        await graph.ShortestPathBfs(0).Should().BeEquivalentOrderTo([0u, 1u, 1u, 1u, 1u]);
        await graph.ShortestPathBfs(1).Should().BeEquivalentOrderTo([3u, 0u, 1u, 2u, 2u]);
        await graph.ShortestPathBfs(2).Should().BeEquivalentOrderTo([2u, 3u, 0u, 1u, 1u]);
        await graph.ShortestPathBfs(3).Should().BeEquivalentOrderTo([4294967295u, 4294967295u, 4294967295u, 0u, 4294967295u]);
        await graph.ShortestPathBfs(4).Should().BeEquivalentOrderTo([1u, 2u, 2u, 1u, 0u]);

        await graph.ShortestPathBfsReverse(0).Should().BeEquivalentOrderTo([0u, 3u, 2u, 4294967295u, 1u]);
        await graph.ShortestPathBfsReverse(1).Should().BeEquivalentOrderTo([1u, 0u, 3u, 4294967295u, 2u]);
        await graph.ShortestPathBfsReverse(2).Should().BeEquivalentOrderTo([1u, 1u, 0u, 4294967295u, 2u]);
        await graph.ShortestPathBfsReverse(3).Should().BeEquivalentOrderTo([1u, 2u, 1u, 0u, 1u]);
        await graph.ShortestPathBfsReverse(4).Should().BeEquivalentOrderTo([1u, 2u, 1u, 4294967295u, 0u]);
    }
    [Test, MultipleAssertions]
    public async Task 重み付きグラフ()
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
        await graph.ShortestPathBfs(0).Should().BeEquivalentOrderTo([0u, 1u, 1u, 1u, 1u]);
        await graph.ShortestPathBfs(1).Should().BeEquivalentOrderTo([3u, 0u, 1u, 2u, 2u]);
        await graph.ShortestPathBfs(2).Should().BeEquivalentOrderTo([2u, 3u, 0u, 1u, 1u]);
        await graph.ShortestPathBfs(3).Should().BeEquivalentOrderTo([4294967295u, 4294967295u, 4294967295u, 0u, 4294967295u]);
        await graph.ShortestPathBfs(4).Should().BeEquivalentOrderTo([1u, 2u, 2u, 1u, 0u]);

        await graph.ShortestPathBfsReverse(0).Should().BeEquivalentOrderTo([0u, 3u, 2u, 4294967295u, 1u]);
        await graph.ShortestPathBfsReverse(1).Should().BeEquivalentOrderTo([1u, 0u, 3u, 4294967295u, 2u]);
        await graph.ShortestPathBfsReverse(2).Should().BeEquivalentOrderTo([1u, 1u, 0u, 4294967295u, 2u]);
        await graph.ShortestPathBfsReverse(3).Should().BeEquivalentOrderTo([1u, 2u, 1u, 0u, 1u]);
        await graph.ShortestPathBfsReverse(4).Should().BeEquivalentOrderTo([1u, 2u, 1u, 4294967295u, 0u]);
    }
}