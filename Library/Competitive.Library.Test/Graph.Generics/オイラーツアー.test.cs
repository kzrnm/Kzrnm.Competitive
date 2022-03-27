using FluentAssertions;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    // verification-helper: SAMEAS Library/run.test.py
    public class オイラーツアーTests
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
            tour.Events.Should().Equal(
                new オイラーツアー<GraphEdge>.Event(-1, new GraphEdge(0), true),
                new オイラーツアー<GraphEdge>.Event(0, new GraphEdge(1), true),
                new オイラーツアー<GraphEdge>.Event(1, new GraphEdge(3), true),
                new オイラーツアー<GraphEdge>.Event(3, new GraphEdge(7), true),
                new オイラーツアー<GraphEdge>.Event(3, new GraphEdge(7), false),
                new オイラーツアー<GraphEdge>.Event(1, new GraphEdge(3), false),
                new オイラーツアー<GraphEdge>.Event(1, new GraphEdge(4), true),
                new オイラーツアー<GraphEdge>.Event(1, new GraphEdge(4), false),
                new オイラーツアー<GraphEdge>.Event(0, new GraphEdge(1), false),
                new オイラーツアー<GraphEdge>.Event(0, new GraphEdge(2), true),
                new オイラーツアー<GraphEdge>.Event(2, new GraphEdge(5), true),
                new オイラーツアー<GraphEdge>.Event(2, new GraphEdge(5), false),
                new オイラーツアー<GraphEdge>.Event(2, new GraphEdge(6), true),
                new オイラーツアー<GraphEdge>.Event(2, new GraphEdge(6), false),
                new オイラーツアー<GraphEdge>.Event(0, new GraphEdge(2), false),
                new オイラーツアー<GraphEdge>.Event(-1, new GraphEdge(0), false));
            Enumerable.Range(0, 8).Select(i => tour[i]).Should().Equal(
                (0, 15), (1, 8), (9, 14), (2, 5), (6, 7), (10, 11), (12, 13), (3, 4));

            var lca = tree.LowestCommonAncestor();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var expectedLca = lca.Lca(i, j);
                    tour.LowestCommonAncestor(i, j).Should().Be(expectedLca, "Lca {0} and {1} → {2}", i, j, expectedLca);
                }
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
            tour.Events.Should().Equal(
                new オイラーツアー<WEdge<int>>.Event(-1, new WEdge<int>(0, 0), true),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(1, 1), true),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(3, 3), true),
                new オイラーツアー<WEdge<int>>.Event(3, new WEdge<int>(7, 7), true),
                new オイラーツアー<WEdge<int>>.Event(3, new WEdge<int>(7, 7), false),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(3, 3), false),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(4, 4), true),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(4, 4), false),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(1, 1), false),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(2, 2), true),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(5, 5), true),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(5, 5), false),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(6, 6), true),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(6, 6), false),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(2, 2), false),
                new オイラーツアー<WEdge<int>>.Event(-1, new WEdge<int>(0, 0), false));
            Enumerable.Range(0, 8).Select(i => tour[i]).Should().Equal(
                (0, 15),
                (1, 8), (9, 14), (2, 5), (6, 7), (10, 11), (12, 13), (3, 4));

            var lca = tree.LowestCommonAncestor();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    tour.LowestCommonAncestor(i, j).Should().Be(lca.Lca(i, j));
        }
    }
}
