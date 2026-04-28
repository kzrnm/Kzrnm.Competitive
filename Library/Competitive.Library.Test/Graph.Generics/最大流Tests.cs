
namespace Kzrnm.Competitive.Testing.Graph;

public class 最大流Tests
{
    [Test, MultipleAssertions]
    public async Task Int()
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
        var graph = gb.ToMFGraph();
        await graph.Flow(0, 1).Should().BeEqualTo(1);
        await graph.Flow(0, 3).Should().BeEqualTo(46);
    }

    [Test, MultipleAssertions]
    public async Task Long()
    {
        var gb = new WLongGraphBuilder(5, true);
        gb.Add(0, 1, 1);
        gb.Add(0, 2, 10);
        gb.Add(0, 3, 30);
        gb.Add(0, 4, 40);
        gb.Add(1, 2, 5);
        gb.Add(2, 3, 605);
        gb.Add(2, 4, 6);
        gb.Add(4, 3, 6);
        gb.Add(4, 0, 1);
        var graph = gb.ToMFGraph();
        await graph.Flow(0, 1).Should().BeEqualTo(1);
        await graph.Flow(0, 3).Should().BeEqualTo(46);
    }
}