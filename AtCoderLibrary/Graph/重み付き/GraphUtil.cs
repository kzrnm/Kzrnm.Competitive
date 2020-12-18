using System;
using System.Collections.Generic;
using System.Linq;
using static AtCoder.Global;


namespace AtCoder.Graph
{
    public static class WGraphUtil
    {
        public static int MaxFlow(this WNode<int>[] graph, int from, int to)
            => MaxFlow<int, IntOperator>(graph, from, to);
        public static long MaxFlow(this WNode<long>[] graph, int from, int to)
            => MaxFlow<long, LongOperator>(graph, from, to);
        public static T MaxFlow<T, TOp>(this WNode<T>[] graph, int from, int to)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            TOp op = default;
            var capacities = new Dictionary<(int from, int to), T>();
            for (int i = 0; i < graph.Length; i++)
                foreach (var next in graph[i].children)
                    capacities.Add((i, next.to), next.value);

            var children = new HashSet<int>[graph.Length];
            for (int i = 0; i < children.Length; i++)
                children[i] = new HashSet<int>(graph[i].children.Select(c => c.to));
            T ret = default;
            while (true)
            {
                var routes = new (int from, int to)[children.Length][];
                var queue = new Queue<int>(children.Length);
                routes[from] = Array.Empty<ValueTuple<int, int>>();
                queue.Enqueue(from);
                while (queue.Count > 0 && routes[to] == null)
                {
                    var cur = queue.Dequeue();
                    foreach (var child in children[cur])
                    {
                        var route = (cur, child);
                        if (routes[child] == null && op.GreaterThan(capacities[route], default))
                        {
                            routes[child] = new (int from, int to)[routes[cur].Length + 1];
                            routes[cur].CopyTo(routes[child], 0);
                            routes[child][^1] = route;
                            queue.Enqueue(child);
                        }
                    }
                }
                if (routes[to] == null)
                    return ret;

                var min = op.MaxValue;
                foreach (var route in routes[to])
                {
                    if (op.GreaterThan(min, capacities[route]))
                        min = capacities[route];
                }

                ret = op.Add(min, ret);
                foreach (var route in routes[to])
                {
                    capacities[route] = op.Subtract(capacities[route], min);

                    var rev = (route.to, route.from);
                    children[route.to].Add(route.from);
                    capacities.TryGetValue(rev, out T v);
                    capacities[rev] = op.Add(v, min);
                }
            }
        }

        public static int[][] WarshallFloyd(this WNode<int>[] graph)
            => WarshallFloyd<int, IntOperator>(graph);
        public static long[][] WarshallFloyd(this WNode<long>[] graph)
            => WarshallFloyd<long, LongOperator>(graph);
        public static T[][] WarshallFloyd<T, TOp>(this WNode<T>[] graph)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            TOp op = default;
            var INF = op.Divide(op.MaxValue, op.Increment(op.Increment(default)));
            var res = Global.NewArray(graph.Length, graph.Length, default(T));
            for (var i = 0; i < graph.Length; i++)
            {
                for (var j = 0; j < graph.Length; j++)
                {
                    res[i][j] = INF;
                }
                res[i][i] = default;
                foreach (var next in graph[i].children)
                    res[i][next.to] = next.value;
            }
            for (var k = 0; k < graph.Length; k++)
                for (var i = 0; i < graph.Length; i++)
                    for (var j = 0; j < graph.Length; j++)
                    {
                        var x = op.Add(res[i][k], res[k][j]);
                        if (op.GreaterThan(res[i][j], x))
                            res[i][j] = x;
                    }
            return res;
        }

        public static int[] Dijkstra(this WNode<int>[] graph, int start)
            => Dijkstra<int, IntOperator>(graph, start);
        public static long[] Dijkstra(this WNode<long>[] graph, int start)
            => Dijkstra<long, LongOperator>(graph, start);
        public static T[] Dijkstra<T, TOp>(this WNode<T>[] graph, int start)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            TOp op = default;
            var INF = op.Divide(op.MaxValue, op.Increment(op.Increment(default)));
            var res = Global.NewArray(graph.Length, INF);
            res[start] = default;

            var used = new bool[graph.Length];
            int count = 0;
            var remains = new PriorityQueue<T, int>();
            remains.Add(default, start);

            while (remains.Count > 0)
            {
                var (len, ix) = remains.Dequeue();
                if (used[ix]) continue;
                used[ix] = true;
                if (++count >= graph.Length) break;
                foreach (var next in graph[ix].children)
                {
                    var nextLength = op.Add(len, next.value);
                    if (op.GreaterThan(res[next.to], nextLength))
                        remains.Add(res[next.to] = nextLength, next.to);
                }
            }
            return res;
        }

        public static int[] BellmanFord(this WNode<int>[] graph, int start)
            => BellmanFord<int, IntOperator>(graph, start);
        public static long[] BellmanFord(this WNode<long>[] graph, int start)
            => BellmanFord<long, LongOperator>(graph, start);
        public static T[] BellmanFord<T, TOp>(this WNode<T>[] graph, int start)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            TOp op = default;
            var INF = op.Divide(op.MaxValue, op.Increment(op.Increment(default)));
            var res = Global.NewArray(graph.Length, INF);
            res[start] = default;

            for (int i = 1; i <= graph.Length; i++)
                foreach (var node in graph)
                    foreach (var next in node.children)
                    {
                        var x = op.Add(res[node.index], next.value);
                        if (op.GreaterThan(res[next.to], x))
                        {
                            res[next.to] = x;
                            if (i == graph.Length)
                                throw new InvalidOperationException("負の閉路");
                        }
                    }
            return res;
        }
    }
}