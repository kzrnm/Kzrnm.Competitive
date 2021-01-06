using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoder.Graph
{
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
            var tour = gb.ToTree().EulerianTour();
            tour.Events.Should().Equal(
                new オイラーツアー<Edge>.Event(-1, new Edge(0), false),
                new オイラーツアー<Edge>.Event(0, new Edge(1), false),
                new オイラーツアー<Edge>.Event(1, new Edge(3), false),
                new オイラーツアー<Edge>.Event(3, new Edge(7), false),
                new オイラーツアー<Edge>.Event(3, new Edge(7), true),
                new オイラーツアー<Edge>.Event(1, new Edge(3), true),
                new オイラーツアー<Edge>.Event(1, new Edge(4), false),
                new オイラーツアー<Edge>.Event(1, new Edge(4), true),
                new オイラーツアー<Edge>.Event(0, new Edge(1), true),
                new オイラーツアー<Edge>.Event(0, new Edge(2), false),
                new オイラーツアー<Edge>.Event(2, new Edge(5), false),
                new オイラーツアー<Edge>.Event(2, new Edge(5), true),
                new オイラーツアー<Edge>.Event(2, new Edge(6), false),
                new オイラーツアー<Edge>.Event(2, new Edge(6), true),
                new オイラーツアー<Edge>.Event(0, new Edge(2), true),
                new オイラーツアー<Edge>.Event(-1, new Edge(0), true));
            Enumerable.Range(0, 8).Select(i => tour[i]).Should().Equal(
                (0, 15), (1, 8), (9, 14), (2, 5), (6, 7), (10, 11), (12, 13), (3, 4));
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
            var tour = gb.ToTree().EulerianTour();
            tour.Events.Should().Equal(
                new オイラーツアー<WEdge<int>>.Event(-1, new WEdge<int>(0, 0), false),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(1, 1), false),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(3, 3), false),
                new オイラーツアー<WEdge<int>>.Event(3, new WEdge<int>(7, 7), false),
                new オイラーツアー<WEdge<int>>.Event(3, new WEdge<int>(7, 7), true),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(3, 3), true),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(4, 4), false),
                new オイラーツアー<WEdge<int>>.Event(1, new WEdge<int>(4, 4), true),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(1, 1), true),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(2, 2), false),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(5, 5), false),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(5, 5), true),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(6, 6), false),
                new オイラーツアー<WEdge<int>>.Event(2, new WEdge<int>(6, 6), true),
                new オイラーツアー<WEdge<int>>.Event(0, new WEdge<int>(2, 2), true),
                new オイラーツアー<WEdge<int>>.Event(-1, new WEdge<int>(0, 0), true));
            Enumerable.Range(0, 8).Select(i => tour[i]).Should().Equal(
                (0, 15),
                (1, 8), (9, 14), (2, 5), (6, 7), (10, 11), (12, 13), (3, 4));
        }
    }
}
