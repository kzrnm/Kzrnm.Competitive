
namespace Kzrnm.Competitive.Testing.Graph
{
    public class 木のノードの距離Tests
    {
        [Fact]
        public void 重みなしグラフ()
        {
            var gb = new GraphBuilder(5, false);
            gb.Add(0, 1);
            gb.Add(1, 2);
            gb.Add(1, 3);
            gb.Add(1, 4);

            var tree = gb.ToTree();
            tree.Distance(0, 0).Should().Be(0);
            tree.Distance(0, 1).Should().Be(1);
            tree.Distance(0, 2).Should().Be(2);
            tree.Distance(0, 3).Should().Be(2);
            tree.Distance(0, 4).Should().Be(2);
            tree.Distance(1, 0).Should().Be(1);
            tree.Distance(1, 1).Should().Be(0);
            tree.Distance(1, 2).Should().Be(1);
            tree.Distance(1, 3).Should().Be(1);
            tree.Distance(1, 4).Should().Be(1);
            tree.Distance(2, 0).Should().Be(2);
            tree.Distance(2, 1).Should().Be(1);
            tree.Distance(2, 2).Should().Be(0);
            tree.Distance(2, 3).Should().Be(2);
            tree.Distance(2, 4).Should().Be(2);
            tree.Distance(3, 0).Should().Be(2);
            tree.Distance(3, 1).Should().Be(1);
            tree.Distance(3, 2).Should().Be(2);
            tree.Distance(3, 3).Should().Be(0);
            tree.Distance(3, 4).Should().Be(2);
            tree.Distance(4, 0).Should().Be(2);
            tree.Distance(4, 1).Should().Be(1);
            tree.Distance(4, 2).Should().Be(2);
            tree.Distance(4, 3).Should().Be(2);
            tree.Distance(4, 4).Should().Be(0);
        }
        [Fact]
        public void 重み付きグラフ()
        {
            var gb = new WIntGraphBuilder(5, false);
            gb.Add(0, 1, 1);
            gb.Add(1, 2, 10);
            gb.Add(1, 3, 30);
            gb.Add(1, 4, 40);
            var tree = gb.ToTree();
            tree.DistanceLength(0, 0).Should().Be(0);
            tree.DistanceLength(0, 1).Should().Be(1);
            tree.DistanceLength(0, 2).Should().Be(11);
            tree.DistanceLength(0, 3).Should().Be(31);
            tree.DistanceLength(0, 4).Should().Be(41);
            tree.DistanceLength(1, 0).Should().Be(1);
            tree.DistanceLength(1, 1).Should().Be(0);
            tree.DistanceLength(1, 2).Should().Be(10);
            tree.DistanceLength(1, 3).Should().Be(30);
            tree.DistanceLength(1, 4).Should().Be(40);
            tree.DistanceLength(2, 0).Should().Be(11);
            tree.DistanceLength(2, 1).Should().Be(10);
            tree.DistanceLength(2, 2).Should().Be(0);
            tree.DistanceLength(2, 3).Should().Be(40);
            tree.DistanceLength(2, 4).Should().Be(50);
            tree.DistanceLength(3, 0).Should().Be(31);
            tree.DistanceLength(3, 1).Should().Be(30);
            tree.DistanceLength(3, 2).Should().Be(40);
            tree.DistanceLength(3, 3).Should().Be(0);
            tree.DistanceLength(3, 4).Should().Be(70);
            tree.DistanceLength(4, 0).Should().Be(41);
            tree.DistanceLength(4, 1).Should().Be(40);
            tree.DistanceLength(4, 2).Should().Be(50);
            tree.DistanceLength(4, 3).Should().Be(70);
            tree.DistanceLength(4, 4).Should().Be(0);

            tree.Distance(0, 0).Should().Be(0);
            tree.Distance(0, 1).Should().Be(1);
            tree.Distance(0, 2).Should().Be(2);
            tree.Distance(0, 3).Should().Be(2);
            tree.Distance(0, 4).Should().Be(2);
            tree.Distance(1, 0).Should().Be(1);
            tree.Distance(1, 1).Should().Be(0);
            tree.Distance(1, 2).Should().Be(1);
            tree.Distance(1, 3).Should().Be(1);
            tree.Distance(1, 4).Should().Be(1);
            tree.Distance(2, 0).Should().Be(2);
            tree.Distance(2, 1).Should().Be(1);
            tree.Distance(2, 2).Should().Be(0);
            tree.Distance(2, 3).Should().Be(2);
            tree.Distance(2, 4).Should().Be(2);
            tree.Distance(3, 0).Should().Be(2);
            tree.Distance(3, 1).Should().Be(1);
            tree.Distance(3, 2).Should().Be(2);
            tree.Distance(3, 3).Should().Be(0);
            tree.Distance(3, 4).Should().Be(2);
            tree.Distance(4, 0).Should().Be(2);
            tree.Distance(4, 1).Should().Be(1);
            tree.Distance(4, 2).Should().Be(2);
            tree.Distance(4, 3).Should().Be(2);
            tree.Distance(4, 4).Should().Be(0);
        }
    }
}
