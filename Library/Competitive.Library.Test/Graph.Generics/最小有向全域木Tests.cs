
namespace Kzrnm.Competitive.Testing.Graph;

public class 最小有向全域木Tests
{
    [Test, MultipleAssertions]
    public async Task Int()
    {
        var gb = new WIntGraphBuilder(5, true);
        gb.Add(0, 1, 442);
        gb.Add(0, 2, 104);
        gb.Add(0, 3, 16);
        gb.Add(0, 4, 5524);
        gb.Add(0, 4, 423);
        gb.Add(4, 0, 14214);
        gb.Add(1, 4, 44223);
        gb.Add(3, 4, 5024224);
        gb.Add(2, 3, 42110);
        gb.Add(4, 1, 4424223);
        var graph = gb.ToGraph();
        var mst = graph.DirectedMinimumSpanningTree(0);
        await mst.Edges.Should().BeStrictlyEquivalentTo([
            (0, new WEdge<int>(4, 423)),
            (0, new WEdge<int>(3, 16)),
            (0, new WEdge<int>(2, 104)),
            (0, new WEdge<int>(1, 442)),
        ]);
        await mst.Cost.Should().BeEqualTo(423 + 16 + 104 + 442);

        mst = graph.DirectedMinimumSpanningTree(3);
        await mst.Edges.Should().BeStrictlyEquivalentTo([
            (3, new WEdge<int>(4, 5024224)),
            (0, new WEdge<int>(2, 104)),
            (0, new WEdge<int>(1, 442)),
            (4, new WEdge<int>(0, 14214)),
        ]);
        await mst.Cost.Should().BeEqualTo(5024224 + 104 + 442 + 14214);
    }

    [Test, MultipleAssertions]
    public async Task Long()
    {
        var gb = new WLongGraphBuilder(5, true);
        gb.Add(0, 1, 442);
        gb.Add(0, 2, 104);
        gb.Add(0, 3, 16);
        gb.Add(0, 4, 5524);
        gb.Add(0, 4, 423);
        gb.Add(4, 0, 14214);
        gb.Add(1, 4, 44223);
        gb.Add(3, 4, 5024224);
        gb.Add(2, 3, 42110);
        gb.Add(4, 1, 4424223);
        var graph = gb.ToGraph();
        var mst = graph.DirectedMinimumSpanningTree(0);
        await mst.Edges.Should().BeStrictlyEquivalentTo([
            (0, new WEdge<long>(4, 423)),
            (0, new WEdge<long>(3, 16)),
            (0, new WEdge<long>(2, 104)),
            (0, new WEdge<long>(1, 442)),
        ]);
        await mst.Cost.Should().BeEqualTo(423 + 16 + 104 + 442);

        mst = graph.DirectedMinimumSpanningTree(3);
        await mst.Edges.Should().BeStrictlyEquivalentTo([
            (3, new WEdge<long>(4, 5024224)),
            (0, new WEdge<long>(2, 104)),
            (0, new WEdge<long>(1, 442)),
            (4, new WEdge<long>(0, 14214)),
        ]);
        await mst.Cost.Should().BeEqualTo(5024224 + 104 + 442 + 14214);
    }
}