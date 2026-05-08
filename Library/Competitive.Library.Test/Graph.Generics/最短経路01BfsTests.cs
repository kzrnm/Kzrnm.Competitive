
namespace Kzrnm.Competitive.Testing.Graph;

public class 最短経路01BfsTests
{
    [Test, MultipleAssertions]
    public async Task Int()
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
        await graph.ShortestPath01Bfs(0).Should().BeStrictlyEquivalentTo([0, 1, 0, 1, 0]);
        await graph.ShortestPath01Bfs(1).Should().BeStrictlyEquivalentTo([1, 0, 0, 1, 0]);
        await graph.ShortestPath01Bfs(2).Should().BeStrictlyEquivalentTo([1, 2, 0, 1, 0]);
        await graph.ShortestPath01Bfs(3).Should().BeStrictlyEquivalentTo([int.MaxValue, int.MaxValue, int.MaxValue, 0, int.MaxValue]);
        await graph.ShortestPath01Bfs(4).Should().BeStrictlyEquivalentTo([1, 2, 1, 1, 0]);
    }

    [Test, MultipleAssertions]
    public async Task Long()
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
        await graph.ShortestPath01Bfs(0).Should().BeStrictlyEquivalentTo([0L, 1, 0, 1, 0]);
        await graph.ShortestPath01Bfs(1).Should().BeStrictlyEquivalentTo([1L, 0, 0, 1, 0]);
        await graph.ShortestPath01Bfs(2).Should().BeStrictlyEquivalentTo([1L, 2, 0, 1, 0]);
        await graph.ShortestPath01Bfs(3).Should().BeStrictlyEquivalentTo([long.MaxValue, long.MaxValue, long.MaxValue, 0, long.MaxValue]);
        await graph.ShortestPath01Bfs(4).Should().BeStrictlyEquivalentTo([1L, 2, 1, 1, 0]);
    }
}