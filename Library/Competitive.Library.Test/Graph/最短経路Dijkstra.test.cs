using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class 最短経路DijkstraTests
    {
        [Fact]
        public void Int()
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
            var graph = gb.ToGraph();
            graph.Dijkstra(0).Should().Equal(new int[] { 0, 1, 6, 18, 12 });
            graph.Dijkstra(1).Should().Equal(new int[] { 12, 0, 5, 17, 11 });
            graph.Dijkstra(2).Should().Equal(new int[] { 7, 8, 0, 12, 6 });
            graph.Dijkstra(3).Should().Equal(new int[] { int.MaxValue, int.MaxValue, int.MaxValue, 0, int.MaxValue });
            graph.Dijkstra(4).Should().Equal(new int[] { 1, 2, 7, 6, 0 });
        }

        [Fact]
        public void Long()
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
            var graph = gb.ToGraph();
            graph.Dijkstra(0).Should().Equal(new long[] { 0, 1, 6, 18, 12 });
            graph.Dijkstra(1).Should().Equal(new long[] { 12, 0, 5, 17, 11 });
            graph.Dijkstra(2).Should().Equal(new long[] { 7, 8, 0, 12, 6 });
            graph.Dijkstra(3).Should().Equal(new long[] { long.MaxValue, long.MaxValue, long.MaxValue, 0, long.MaxValue });
            graph.Dijkstra(4).Should().Equal(new long[] { 1, 2, 7, 6, 0 });
        }
    }
}
