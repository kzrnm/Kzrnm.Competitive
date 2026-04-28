
namespace Kzrnm.Competitive.Testing.Graph;

public class 木のノードの距離Tests
{
    [Test, MultipleAssertions]
    public async Task 重みなしグラフ()
    {
        var gb = new GraphBuilder(5, false);
        gb.Add(0, 1);
        gb.Add(1, 2);
        gb.Add(1, 3);
        gb.Add(1, 4);

        var tree = gb.ToTree();
        await tree.Distance(0, 0).Should().BeEqualTo(0);
        await tree.Distance(0, 1).Should().BeEqualTo(1);
        await tree.Distance(0, 2).Should().BeEqualTo(2);
        await tree.Distance(0, 3).Should().BeEqualTo(2);
        await tree.Distance(0, 4).Should().BeEqualTo(2);
        await tree.Distance(1, 0).Should().BeEqualTo(1);
        await tree.Distance(1, 1).Should().BeEqualTo(0);
        await tree.Distance(1, 2).Should().BeEqualTo(1);
        await tree.Distance(1, 3).Should().BeEqualTo(1);
        await tree.Distance(1, 4).Should().BeEqualTo(1);
        await tree.Distance(2, 0).Should().BeEqualTo(2);
        await tree.Distance(2, 1).Should().BeEqualTo(1);
        await tree.Distance(2, 2).Should().BeEqualTo(0);
        await tree.Distance(2, 3).Should().BeEqualTo(2);
        await tree.Distance(2, 4).Should().BeEqualTo(2);
        await tree.Distance(3, 0).Should().BeEqualTo(2);
        await tree.Distance(3, 1).Should().BeEqualTo(1);
        await tree.Distance(3, 2).Should().BeEqualTo(2);
        await tree.Distance(3, 3).Should().BeEqualTo(0);
        await tree.Distance(3, 4).Should().BeEqualTo(2);
        await tree.Distance(4, 0).Should().BeEqualTo(2);
        await tree.Distance(4, 1).Should().BeEqualTo(1);
        await tree.Distance(4, 2).Should().BeEqualTo(2);
        await tree.Distance(4, 3).Should().BeEqualTo(2);
        await tree.Distance(4, 4).Should().BeEqualTo(0);
    }
    [Test, MultipleAssertions]
    public async Task 重み付きグラフ()
    {
        var gb = new WIntGraphBuilder(5, false);
        gb.Add(0, 1, 1);
        gb.Add(1, 2, 10);
        gb.Add(1, 3, 30);
        gb.Add(1, 4, 40);
        var tree = gb.ToTree();
        await tree.DistanceLength(0, 0).Should().BeEqualTo(0);
        await tree.DistanceLength(0, 1).Should().BeEqualTo(1);
        await tree.DistanceLength(0, 2).Should().BeEqualTo(11);
        await tree.DistanceLength(0, 3).Should().BeEqualTo(31);
        await tree.DistanceLength(0, 4).Should().BeEqualTo(41);
        await tree.DistanceLength(1, 0).Should().BeEqualTo(1);
        await tree.DistanceLength(1, 1).Should().BeEqualTo(0);
        await tree.DistanceLength(1, 2).Should().BeEqualTo(10);
        await tree.DistanceLength(1, 3).Should().BeEqualTo(30);
        await tree.DistanceLength(1, 4).Should().BeEqualTo(40);
        await tree.DistanceLength(2, 0).Should().BeEqualTo(11);
        await tree.DistanceLength(2, 1).Should().BeEqualTo(10);
        await tree.DistanceLength(2, 2).Should().BeEqualTo(0);
        await tree.DistanceLength(2, 3).Should().BeEqualTo(40);
        await tree.DistanceLength(2, 4).Should().BeEqualTo(50);
        await tree.DistanceLength(3, 0).Should().BeEqualTo(31);
        await tree.DistanceLength(3, 1).Should().BeEqualTo(30);
        await tree.DistanceLength(3, 2).Should().BeEqualTo(40);
        await tree.DistanceLength(3, 3).Should().BeEqualTo(0);
        await tree.DistanceLength(3, 4).Should().BeEqualTo(70);
        await tree.DistanceLength(4, 0).Should().BeEqualTo(41);
        await tree.DistanceLength(4, 1).Should().BeEqualTo(40);
        await tree.DistanceLength(4, 2).Should().BeEqualTo(50);
        await tree.DistanceLength(4, 3).Should().BeEqualTo(70);
        await tree.DistanceLength(4, 4).Should().BeEqualTo(0);

        await tree.Distance(0, 0).Should().BeEqualTo(0);
        await tree.Distance(0, 1).Should().BeEqualTo(1);
        await tree.Distance(0, 2).Should().BeEqualTo(2);
        await tree.Distance(0, 3).Should().BeEqualTo(2);
        await tree.Distance(0, 4).Should().BeEqualTo(2);
        await tree.Distance(1, 0).Should().BeEqualTo(1);
        await tree.Distance(1, 1).Should().BeEqualTo(0);
        await tree.Distance(1, 2).Should().BeEqualTo(1);
        await tree.Distance(1, 3).Should().BeEqualTo(1);
        await tree.Distance(1, 4).Should().BeEqualTo(1);
        await tree.Distance(2, 0).Should().BeEqualTo(2);
        await tree.Distance(2, 1).Should().BeEqualTo(1);
        await tree.Distance(2, 2).Should().BeEqualTo(0);
        await tree.Distance(2, 3).Should().BeEqualTo(2);
        await tree.Distance(2, 4).Should().BeEqualTo(2);
        await tree.Distance(3, 0).Should().BeEqualTo(2);
        await tree.Distance(3, 1).Should().BeEqualTo(1);
        await tree.Distance(3, 2).Should().BeEqualTo(2);
        await tree.Distance(3, 3).Should().BeEqualTo(0);
        await tree.Distance(3, 4).Should().BeEqualTo(2);
        await tree.Distance(4, 0).Should().BeEqualTo(2);
        await tree.Distance(4, 1).Should().BeEqualTo(1);
        await tree.Distance(4, 2).Should().BeEqualTo(2);
        await tree.Distance(4, 3).Should().BeEqualTo(2);
        await tree.Distance(4, 4).Should().BeEqualTo(0);
    }
}