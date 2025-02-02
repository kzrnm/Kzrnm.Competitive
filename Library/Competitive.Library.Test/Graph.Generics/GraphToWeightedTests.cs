namespace Kzrnm.Competitive.Testing.Graph;

public class GraphToWeightedTests
{
    [Fact]
    public void Dijkstra()
    {
        var gb = new GraphBuilder(5, true);
        gb.Add(0, 1);
        gb.Add(2, 1);
        gb.Add(1, 3);
        gb.Add(3, 2);
        gb.Add(4, 2);
        gb.ToGraph().ToWeighted().Dijkstra(0).ShouldBe([0u, 1u, 3u, 2u, uint.MaxValue]);
    }

    [Fact]
    public void Scc()
    {
        var gb = new GraphBuilder(5, true);
        gb.Add(0, 1);
        gb.Add(2, 1);
        gb.Add(1, 3);
        gb.Add(3, 2);
        gb.Add(4, 2);
        gb.ToGraph().ToWeighted().SccIds().ids.ShouldBe([1, 2, 2, 2, 0]);
    }
}