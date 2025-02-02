using System.Linq;

namespace Kzrnm.Competitive.Testing.Graph;

public class 帰りもあるオイラーツアーTests
{
    [Fact]
    public void 重みなしグラフ()
    {
        var gb = new GraphBuilder(8, false);
        gb.Add(0, 1);
        gb.Add(0, 2);
        gb.Add(1, 3);
        gb.Add(1, 4);
        gb.Add(2, 5);
        gb.Add(2, 6);
        gb.Add(3, 7);
        var tree = gb.ToTree();
        var tour = tree.EulerianTour();
        tour.Events.ShouldBe([
            new 帰りもあるオイラーツアー<GraphEdge>.Event(-1, new GraphEdge(0), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(0, new GraphEdge(1), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(1, new GraphEdge(3), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(3, new GraphEdge(7), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(3, new GraphEdge(7), false),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(1, new GraphEdge(3), false),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(1, new GraphEdge(4), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(1, new GraphEdge(4), false),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(0, new GraphEdge(1), false),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(0, new GraphEdge(2), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(2, new GraphEdge(6), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(2, new GraphEdge(6), false),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(2, new GraphEdge(5), true),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(2, new GraphEdge(5), false),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(0, new GraphEdge(2), false),
            new 帰りもあるオイラーツアー<GraphEdge>.Event(-1, new GraphEdge(0), false)
        ]);
        Enumerable.Range(0, 8).Select(i => tour[i]).ShouldBe([
            (0, 15), (1, 8), (9, 14), (2, 5), (6, 7), (12, 13), (10, 11), (3, 4)
        ]);
    }
    [Fact]
    public void 重み付きグラフ()
    {
        var gb = new WIntGraphBuilder(8, false);
        gb.Add(0, 1, 1);
        gb.Add(0, 2, 2);
        gb.Add(1, 3, 3);
        gb.Add(1, 4, 4);
        gb.Add(2, 5, 5);
        gb.Add(2, 6, 6);
        gb.Add(3, 7, 7);
        var tree = gb.ToTree();
        var tour = tree.EulerianTour();
        tour.Events.ShouldBe([
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(-1, new WEdge<int>(0, 0), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(1, 1), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(3, 3), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(3, new WEdge<int>(7, 7), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(3, new WEdge<int>(7, 7), false),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(3, 3), false),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(4, 4), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(4, 4), false),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(1, 1), false),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(2, 2), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(6, 6), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(6, 6), false),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(5, 5), true),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(5, 5), false),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(2, 2), false),
            new 帰りもあるオイラーツアー<WEdge<int>>.Event(-1, new WEdge<int>(0, 0), false)
        ]);
        Enumerable.Range(0, 8).Select(i => tour[i]).ShouldBe([
            (0, 15), (1, 8), (9, 14), (2, 5), (6, 7), (12, 13), (10, 11), (3, 4)
        ]);
    }
}
