using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
            graph.Dijkstra(0).Should().Equal(
                0,
                1,
                6,
                18,
                12);
            graph.Dijkstra(1).Should().Equal(
                12,
                0,
                5,
                17,
                11);
            graph.Dijkstra(2).Should().Equal(
                7,
                8,
                0,
                12,
                6);
            graph.Dijkstra(3).Should().Equal(
                int.MaxValue,
                int.MaxValue,
                int.MaxValue,
                0,
                int.MaxValue);
            graph.Dijkstra(4).Should().Equal(
                1,
                2,
                7,
                6,
                0);

            graph.DijkstraWithRoute(0).Should().Equal(
                new[] {
                    (0, new WEdge<int>[0]),
                    (1, new WEdge<int>[]{ new(1, 1) }),
                    (6, new WEdge<int>[]{ new(2, 5), new(1, 1) }),
                    (18, new WEdge<int>[]{ new(3, 6), new(4, 6), new(2, 5), new(1, 1) }),
                    (12, new WEdge<int>[]{ new(4, 6), new(2, 5), new(1, 1) }),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(1).Should().Equal(
                new[] {
                    (12, new WEdge<int>[]{ new(0, 1), new(4, 6), new(2, 5) }),
                    (0, new WEdge<int>[0]),
                    (5, new WEdge<int>[]{ new(2, 5) }),
                    (17, new WEdge<int>[]{ new(3, 6), new(4, 6), new(2, 5) }),
                    (11, new WEdge<int>[]{ new(4, 6), new(2, 5) }),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(2).Should().Equal(
                new[] {
                    (7, new WEdge<int>[]{ new(0, 1), new(4, 6) }),
                    (8, new WEdge<int>[]{ new(1, 1), new(0, 1), new(4, 6) }),
                    (0, new WEdge<int>[0]),
                    (12, new WEdge<int>[]{ new(3, 6), new(4, 6) }),
                    (6, new WEdge<int>[]{ new(4, 6) }),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(3).Should().Equal(
                new[] {
                    (int.MaxValue, null),
                    (int.MaxValue, null),
                    (int.MaxValue, null),
                    (0, new WEdge<int>[0]),
                    (int.MaxValue, null),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(4).Should().Equal(
                new[] {
                    (1, new WEdge<int>[]{ new(0, 1) }),
                    (2, new WEdge<int>[]{ new(1, 1), new(0, 1) }),
                    (7, new WEdge<int>[]{ new(2, 5), new(1, 1), new(0, 1) }),
                    (6, new WEdge<int>[]{ new(3, 6) }),
                    (0, new WEdge<int>[0]),
                }, ResultEqualityCompare);
        }

        [Fact]
        public void Long()
        {
            const long LARGE = 1L << 40;
            var gb = new WLongGraphBuilder(5, true);
            gb.Add(0, 1, LARGE + 1);
            gb.Add(0, 2, LARGE * 9 + 10);
            gb.Add(0, 3, LARGE * 9 + 30);
            gb.Add(0, 4, LARGE * 9 + 40);
            gb.Add(1, 2, LARGE + 5);
            gb.Add(2, 3, LARGE * 10 + 605);
            gb.Add(2, 4, LARGE + 6);
            gb.Add(4, 3, LARGE + 6);
            gb.Add(4, 0, LARGE + 1);
            var graph = gb.ToGraph();
            graph.Dijkstra(0).Should().Equal(
                0,
                1 * LARGE + 1,
                2 * LARGE + 6,
                4 * LARGE + 18,
                3 * LARGE + 12);
            graph.Dijkstra(1).Should().Equal(
                3 * LARGE + 12,
                0L,
                1 * LARGE + 5,
                3 * LARGE + 17,
                2 * LARGE + 11);
            graph.Dijkstra(2).Should().Equal(
                2 * LARGE + 7,
                3 * LARGE + 8,
                0L,
                2 * LARGE + 12,
                1 * LARGE + 6);
            graph.Dijkstra(3).Should().Equal(
                long.MaxValue,
                long.MaxValue,
                long.MaxValue,
                0L,
                long.MaxValue);
            graph.Dijkstra(4).Should().Equal(
                1 * LARGE + 1,
                2 * LARGE + 2,
                3 * LARGE + 7,
                1 * LARGE + 6,
                0L);

            graph.DijkstraWithRoute(0).Should().Equal(
                new[] {
                    (0, new WEdge<long>[0]),
                    (1*LARGE+1, new WEdge<long>[]{ new(1, LARGE+1) }),
                    (2*LARGE+6, new WEdge<long>[]{ new(2, LARGE+5), new(1, LARGE+1) }),
                    (4*LARGE+18, new WEdge<long>[]{ new(3, LARGE+6), new(4, LARGE+6), new(2, LARGE+5), new(1, LARGE+1) }),
                    (3*LARGE+12, new WEdge<long>[]{ new(4, LARGE+6), new(2, LARGE+5), new(1, LARGE+1) }),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(1).Should().Equal(
                new[] {
                    (3*LARGE+12, new WEdge<long>[]{ new(0, LARGE+1), new(4, LARGE+6), new(2, LARGE+5) }),
                    (0L, new WEdge<long>[0]),
                    (1*LARGE+5, new WEdge<long>[]{ new(2, LARGE+5) }),
                    (3*LARGE+17, new WEdge<long>[]{ new(3, LARGE+6), new(4, LARGE+6), new(2, LARGE+5) }),
                    (2*LARGE+11, new WEdge<long>[]{ new(4, LARGE+6), new(2, LARGE+5) }),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(2).Should().Equal(
                new[] {
                    (2*LARGE+7, new WEdge<long>[]{ new(0, LARGE+1), new(4, LARGE+6) }),
                    (3*LARGE+8, new WEdge<long>[]{ new(1, LARGE+1), new(0, LARGE+1), new(4, LARGE+6) }),
                    (0L, new WEdge<long>[0]),
                    (2*LARGE+12, new WEdge<long>[]{ new(3, LARGE+6), new(4, LARGE+6) }),
                    (1*LARGE+6, new WEdge<long>[]{ new(4, LARGE+6) }),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(3).Should().Equal(
                new[] {
                    (long.MaxValue, null),
                    (long.MaxValue, null),
                    (long.MaxValue, null),
                    (0L, new WEdge<long>[0]),
                    (long.MaxValue, null),
                }, ResultEqualityCompare);
            graph.DijkstraWithRoute(4).Should().Equal(
                new[] {
                    (1*LARGE+1, new WEdge<long>[]{ new(0, LARGE+1) }),
                    (2*LARGE+2, new WEdge<long>[]{ new(1, LARGE+1), new(0, LARGE+1) }),
                    (3*LARGE+7, new WEdge<long>[]{ new(2, LARGE+5), new(1, LARGE+1), new(0, LARGE+1) }),
                    (1*LARGE+6, new WEdge<long>[]{ new(3, LARGE+6) }),
                    (0L, new WEdge<long>[0]),
                }, ResultEqualityCompare);
        }

        private static bool ResultEqualityCompare<T>(
            (T Distance, ImmutableStack<WEdge<T>> Route) result,
            (T Distance, WEdge<T>[] Route) expected) where T : struct
        {
            if (!EqualityComparer<T>.Default.Equals(result.Distance, expected.Distance))
                return false;

            return (result.Route is null && expected.Route is null)
                || result.Route.SequenceEqual(expected.Route);
        }
    }
}
