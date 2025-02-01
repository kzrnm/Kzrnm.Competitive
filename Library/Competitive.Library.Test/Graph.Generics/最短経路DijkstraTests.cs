using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Graph;

public class 最短経路DijkstraTests
{
    [Fact]
    public void Random()
    {
        var rnd = new Random(227);
        for (int n = 0; n < 100; n++)
        {
            var size = rnd.Next(10, 60);
            var gb = new WIntGraphBuilder(size, true);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (rnd.Next(100) < 80)
                        gb.Add(i, j, rnd.Next(10000));
                }
            var graph = gb.ToGraph();
            var expecteds = graph.WarshallFloyd();
            for (int i = 0; i < expecteds.Length; i++)
                for (int j = 0; j < expecteds[i].Length; j++)
                    if (expecteds[i][j] == int.MaxValue / 2)
                        expecteds[i][j] = int.MaxValue;

            for (int i = 0; i < size; i++)
            {
                graph.Dijkstra(i).ShouldBe(expecteds[i]);
            }
        }
    }

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
        graph.Dijkstra(0).ShouldBe([
            0,
            1,
            6,
            18,
            12,
        ]);
        graph.Dijkstra(1).ShouldBe([
            12,
            0,
            5,
            17,
            11,
        ]);
        graph.Dijkstra(2).ShouldBe([
            7,
            8,
            0,
            12,
            6,
        ]);
        graph.Dijkstra(3).ShouldBe([
            int.MaxValue,
            int.MaxValue,
            int.MaxValue,
            0,
            int.MaxValue,
        ]);
        graph.Dijkstra(4).ShouldBe([
            1,
            2,
            7,
            6,
            0,
        ]);

        graph.DijkstraWithRoute(0).ToObject().ShouldBe([
            new(0, []),
            new(1, [new(1, 1)]),
            new(6, [new(2, 5), new(1, 1)]),
            new(18, [new(3, 6), new(4, 6), new(2, 5), new(1, 1)]),
            new(12, [new(4, 6), new(2, 5), new(1, 1)]),
        ]);
        graph.DijkstraWithRoute(1).ToObject().ShouldBe([
            new(12, [new(0, 1), new(4, 6), new(2, 5)]),
            new(0, []),
            new(5, [new(2, 5)]),
            new(17, [new(3, 6), new(4, 6), new(2, 5)]),
            new(11, [new(4, 6), new(2, 5)]),
        ]);
        graph.DijkstraWithRoute(2).ToObject().ShouldBe([
            new(7, [new(0, 1), new(4, 6)]),
            new(8, [new(1, 1), new(0, 1), new(4, 6)]),
            new(0, []),
            new(12, [new(3, 6), new(4, 6)]),
            new(6, [new(4, 6)]),
        ]);
        graph.DijkstraWithRoute(3).ToObject().ShouldBe([
            new(int.MaxValue, null),
            new(int.MaxValue, null),
            new(int.MaxValue, null),
            new(0, []),
            new(int.MaxValue, null),
        ]);
        graph.DijkstraWithRoute(4).ToObject().ShouldBe([
            new(1, [new(0, 1)]),
            new(2, [new(1, 1), new(0, 1)]),
            new(7, [new(2, 5), new(1, 1), new(0, 1)]),
            new(6, [new(3, 6)]),
            new(0, []),
        ]);
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
        graph.Dijkstra(0).ShouldBe([
            0,
            1 * LARGE + 1,
            2 * LARGE + 6,
            4 * LARGE + 18,
            3 * LARGE + 12,
        ]);
        graph.Dijkstra(1).ShouldBe([
            3 * LARGE + 12,
            0L,
            1 * LARGE + 5,
            3 * LARGE + 17,
            2 * LARGE + 11,
        ]);
        graph.Dijkstra(2).ShouldBe([
            2 * LARGE + 7,
            3 * LARGE + 8,
            0L,
            2 * LARGE + 12,
            1 * LARGE + 6,
        ]);
        graph.Dijkstra(3).ShouldBe([
            long.MaxValue,
            long.MaxValue,
            long.MaxValue,
            0L,
            long.MaxValue,
        ]);
        graph.Dijkstra(4).ShouldBe([
            1 * LARGE + 1,
            2 * LARGE + 2,
            3 * LARGE + 7,
            1 * LARGE + 6,
            0L,
        ]);

        graph.DijkstraWithRoute(0).ToObject().ShouldBe([
            new(0, []),
            new(1 * LARGE + 1, [new(1, LARGE + 1)]),
            new(2 * LARGE + 6, [new(2, LARGE + 5), new(1, LARGE + 1)]),
            new(4 * LARGE + 18, [new(3, LARGE + 6), new(4, LARGE + 6), new(2, LARGE + 5), new(1, LARGE + 1)]),
            new(3 * LARGE + 12, [new(4, LARGE + 6), new(2, LARGE + 5), new(1, LARGE + 1)]),
        ]);
        graph.DijkstraWithRoute(1).ToObject().ShouldBe([
            new(3 * LARGE + 12, [new(0, LARGE + 1), new(4, LARGE + 6), new(2, LARGE + 5)]),
            new(0L, []),
            new(1 * LARGE + 5, [new(2, LARGE + 5)]),
            new(3 * LARGE + 17, [new(3, LARGE + 6), new(4, LARGE + 6), new(2, LARGE + 5)]),
            new(2 * LARGE + 11, [new(4, LARGE + 6), new(2, LARGE + 5)]),
        ]);
        graph.DijkstraWithRoute(2).ToObject().ShouldBe([
            new(2 * LARGE + 7, [new(0, LARGE + 1), new(4, LARGE + 6)]),
            new(3 * LARGE + 8, [new(1, LARGE + 1), new(0, LARGE + 1), new(4, LARGE + 6)]),
            new(0L, []),
            new(2 * LARGE + 12, [new(3, LARGE + 6), new(4, LARGE + 6)]),
            new(1 * LARGE + 6, [new(4, LARGE + 6)]),
        ]);
        graph.DijkstraWithRoute(3).ToObject().ShouldBe([
            new(long.MaxValue, null),
            new(long.MaxValue, null),
            new(long.MaxValue, null),
            new(0L, []),
            new(long.MaxValue, null),
        ]);
        graph.DijkstraWithRoute(4).ToObject().ShouldBe([
            new(1 * LARGE + 1, [new(0, LARGE + 1)]),
            new(2 * LARGE + 2, [new(1, LARGE + 1), new(0, LARGE + 1)]),
            new(3 * LARGE + 7, [new(2, LARGE + 5), new(1, LARGE + 1), new(0, LARGE + 1)]),
            new(1 * LARGE + 6, [new(3, LARGE + 6)]),
            new(0L, []),
        ]);
    }
}
class DijkstraWithRouteResult<T, TEdge>(T distance, TEdge[] route) : IEquatable<DijkstraWithRouteResult<T, TEdge>>
{
    public T Distance = distance;
    public TEdge[] Route = route;

    public override int GetHashCode()
        => HashCode.Combine(Distance, Route.Length);

    public override bool Equals(object obj)
        => obj is DijkstraWithRouteResult<T, TEdge> other && Equals(other);

    public bool Equals(DijkstraWithRouteResult<T, TEdge> other)
    {
        if (!EqualityComparer<T>.Default.Equals(Distance, other.Distance))
            return false;

        if (Route is null || other.Route is null)
            return Route == other.Route;

        if (Route.Length != other.Route.Length)
            return false;

        for (int i = 0; i < Route.Length; i++)
        {
            if (!EqualityComparer<TEdge>.Default.Equals(Route[i], other.Route[i]))
                return false;
        }
        return true;
    }
}
internal static class DijkstraWithRouteExtension
{
    public static DijkstraWithRouteResult<T, TEdge>[] ToObject<T, TEdge>(this (T Distance, ImmutableStack<TEdge> Route)[] array)
        => array.Select(t => new DijkstraWithRouteResult<T, TEdge>(t.Distance, t.Route?.ToArray())).ToArray();
}
