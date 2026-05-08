
namespace Kzrnm.Competitive.Testing.Graph;

public class 最小全域森KruskalTests
{
    [Test, MultipleAssertions]
    public async Task 重みなしグラフ()
    {
        var gb = new GraphBuilder(5, false);
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
        var res = graph.MinimumSpanningForestKruskal();
        await res.Length.Should().BeEqualTo(1);
        await res[0].Should().BeStrictlyEquivalentTo([
            (0, new GraphEdge(1)),
            (0, new GraphEdge(2)),
            (0, new GraphEdge(3)),
            (0, new GraphEdge(4)),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task 連結ではない重みなし()
    {
        var gb = new GraphBuilder(8, false);
        gb.Add(0, 1);
        gb.Add(0, 2);
        gb.Add(1, 2);
        gb.Add(4, 3);
        gb.Add(4, 7);
        gb.Add(6, 5);
        var graph = gb.ToGraph();
        var res = graph.MinimumSpanningForestKruskal();
        await res.Length.Should().BeEqualTo(3);
        await res[0].Should().BeStrictlyEquivalentTo([
            (0, new GraphEdge(1)),
            (0, new GraphEdge(2)),
        ]);
        await res[1].Should().BeStrictlyEquivalentTo([
            (3, new GraphEdge(4)),
            (4, new GraphEdge(7)),
        ]);
        await res[2].Should().BeStrictlyEquivalentTo([
            (5, new GraphEdge(6)),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task 森の連結重みなし()
    {
        var gb = new GraphBuilder(4, false);
        gb.Add(0, 2);
        gb.Add(1, 3);
        gb.Add(2, 3);
        var graph = gb.ToGraph();
        var res = graph.MinimumSpanningForestKruskal();
        await res.Length.Should().BeEqualTo(1);
        await res[0].Should().BeStrictlyEquivalentTo([
            (0, new GraphEdge(2)),
            (1, new GraphEdge(3)),
            (2, new GraphEdge(3)),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task Int()
    {
        var gb = new WIntGraphBuilder(5, false);
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
        var res = graph.MinimumSpanningForestKruskal();
        await res.Length.Should().BeEqualTo(1);
        await res[0].Should().BeStrictlyEquivalentTo([
            (0, new WEdge<int>(1, 1)),
            (0, new WEdge<int>(4, 1)),
            (1, new WEdge<int>(2, 5)),
            (4, new WEdge<int>(3, 6)),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task Long()
    {
        var gb = new WLongGraphBuilder(5, false);
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
        var res = graph.MinimumSpanningForestKruskal();
        await res.Length.Should().BeEqualTo(1);
        await res[0].Should().BeStrictlyEquivalentTo([
            (0, new WEdge<long>(1, 1)),
            (0, new WEdge<long>(4, 1)),
            (1, new WEdge<long>(2, 5)),
            (4, new WEdge<long>(3, 6)),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task 連結ではない重み付き()
    {
        var gb = new WIntGraphBuilder(8, false);
        gb.Add(0, 1, 1);
        gb.Add(0, 2, 2);
        gb.Add(1, 2, 3);
        gb.Add(4, 3, 4);
        gb.Add(4, 7, 5);
        gb.Add(3, 7, 10);
        gb.Add(6, 5, 6);
        var graph = gb.ToGraph();
        var res = graph.MinimumSpanningForestKruskal();
        await res.Length.Should().BeEqualTo(3);
        await res[0].Should().BeStrictlyEquivalentTo([
            (0, new WEdge<int>(1, 1)),
            (0, new WEdge<int>(2, 2)),
        ]);
        await res[1].Should().BeStrictlyEquivalentTo([
            (3, new WEdge<int>(4, 4)),
            (4, new WEdge<int>(7, 5)),
        ]);
        await res[2].Should().BeStrictlyEquivalentTo([
            (5, new WEdge<int>(6, 6)),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task 森の連結重み付き()
    {
        var gb = new WIntGraphBuilder(4, false);
        gb.Add(0, 2, 1);
        gb.Add(1, 3, 1);
        gb.Add(2, 3, 10);
        var graph = gb.ToGraph();
        var res = graph.MinimumSpanningForestKruskal();
        await res.Length.Should().BeEqualTo(1);
        await res[0].Should().BeStrictlyEquivalentTo([
            (0, new WEdge<int>(2, 1)),
            (1, new WEdge<int>(3, 1)),
            (2, new WEdge<int>(3, 10)),
        ]);
    }
}