
namespace Kzrnm.Competitive.Testing.Graph
{
    public class オイラー路Tests
    {
        [Fact]
        public void 有向グラフ()
        {
            var gb = new GraphBuilder(8, true);
            gb.Add(0, 1);
            gb.Add(1, 2);
            gb.Add(2, 3);
            gb.Add(3, 4);
            gb.Add(4, 5);
            gb.Add(5, 6);
            gb.Add(6, 2);
            gb.Add(2, 4);
            gb.Add(4, 7);
            gb.Add(7, 0);
            var (from, edges) = gb.ToGraph().EulerianTrail();
            from.Should().Be(0);
            edges.Should().Equal([
                new GraphEdge(1),
                new GraphEdge(2),
                new GraphEdge(3),
                new GraphEdge(4),
                new GraphEdge(5),
                new GraphEdge(6),
                new GraphEdge(2),
                new GraphEdge(4),
                new GraphEdge(7),
                new GraphEdge(0),
            ]);
        }

        [Fact]
        public void 無向グラフ()
        {
            var gb = new GraphBuilder(8, false);
            gb.Add(0, 1);
            gb.Add(1, 2);
            gb.Add(2, 3);
            gb.Add(3, 4);
            gb.Add(4, 5);
            gb.Add(5, 6);
            gb.Add(6, 2);
            gb.Add(2, 4);
            gb.Add(4, 7);
            gb.Add(7, 0);
            var (from, edges) = gb.ToGraph().EulerianTrail();
            from.Should().Be(0);
            edges.Should().Equal([
                new GraphEdge(1),
                new GraphEdge(2),
                new GraphEdge(3),
                new GraphEdge(4),
                new GraphEdge(2),
                new GraphEdge(6),
                new GraphEdge(5),
                new GraphEdge(4),
                new GraphEdge(7),
                new GraphEdge(0),
            ]);
        }

        [Fact]
        public void 重み付きグラフ()
        {
            var gb = new WIntGraphBuilder(8, true);
            gb.Add(0, 1, 1);
            gb.Add(1, 2, 2);
            gb.Add(2, 3, 3);
            gb.Add(3, 4, 4);
            gb.Add(4, 5, 5);
            gb.Add(5, 6, 6);
            gb.Add(6, 2, 7);
            gb.Add(2, 4, 8);
            gb.Add(4, 7, 9);
            gb.Add(7, 0, 10);
            var (from, edges) = gb.ToGraph().EulerianTrail();
            from.Should().Be(0);
            edges.Should().Equal([
                new WEdge<int>(1, 1),
                new WEdge<int>(2, 2),
                new WEdge<int>(3, 3),
                new WEdge<int>(4, 4),
                new WEdge<int>(5, 5),
                new WEdge<int>(6, 6),
                new WEdge<int>(2, 7),
                new WEdge<int>(4, 8),
                new WEdge<int>(7, 9),
                new WEdge<int>(0, 10),
            ]);
        }
    }
}
