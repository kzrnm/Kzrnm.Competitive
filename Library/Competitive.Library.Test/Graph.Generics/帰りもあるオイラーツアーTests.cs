namespace Kzrnm.Competitive.Testing.Graph;

public class 帰りもあるオイラーツアーTests
{
    [Test, MultipleAssertions]
    public async Task 重みなしグラフ()
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
        await tour.Events.Should().BeStrictlyEquivalentTo(new 帰りもあるオイラーツアー<GraphEdge>.Event[] {
            new (-1, new (0), true),
            new (0, new (1), true),
            new (1, new (3), true),
            new (3, new (7), true),
            new (3, new (7), false),
            new (1, new (3), false),
            new (1, new (4), true),
            new (1, new (4), false),
            new (0, new (1), false),
            new (0, new (2), true),
            new (2, new (6), true),
            new (2, new (6), false),
            new (2, new (5), true),
            new (2, new (5), false),
            new (0, new (2), false),
            new (-1, new (0), false),
        });
        await Enumerable.Range(0, 8).Select(i => tour[i]).Should().BeStrictlyEquivalentTo([
            (0, 15), (1, 8), (9, 14), (2, 5), (6, 7), (12, 13), (10, 11), (3, 4)
        ]);
    }
    [Test, MultipleAssertions]
    public async Task 重み付きグラフ()
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
        await tour.Events.Should().BeStrictlyEquivalentTo([
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
        await Enumerable.Range(0, 8).Select(i => tour[i]).Should().BeStrictlyEquivalentTo([
            (0, 15), (1, 8), (9, 14), (2, 5), (6, 7), (12, 13), (10, 11), (3, 4)
        ]);
    }
}