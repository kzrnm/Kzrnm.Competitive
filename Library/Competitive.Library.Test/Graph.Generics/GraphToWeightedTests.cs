namespace Kzrnm.Competitive.Testing.Graph;

public class GraphToWeightedTests
{
    [Test]
    public async Task Dijkstra()
    {
        var gb = new GraphBuilder(5, true);
        gb.Add(0, 1);
        gb.Add(2, 1);
        gb.Add(1, 3);
        gb.Add(3, 2);
        gb.Add(4, 2);
        await gb.ToGraph().ToWeighted().Dijkstra(0).Should().BeEquivalentOrderTo([0u, 1u, 3u, 2u, uint.MaxValue]);
    }

    [Test]
    public async Task Scc()
    {
        var gb = new GraphBuilder(5, true);
        gb.Add(0, 1);
        gb.Add(2, 1);
        gb.Add(1, 3);
        gb.Add(3, 2);
        gb.Add(4, 2);
        await gb.ToGraph().ToWeighted().SccIds().ids.Should().BeEquivalentOrderTo([1, 2, 2, 2, 0]);
    }
}