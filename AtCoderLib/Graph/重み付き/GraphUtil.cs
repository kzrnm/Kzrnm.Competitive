using System;
using System.Collections.Generic;
using System.Linq;
using AtCoderProject;
using static AtCoderProject.Global;


static class WGraphUtil
{
    [System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")] class PriorityQueue<T> { protected readonly List<T> data; protected readonly IComparer<T> comparer; public PriorityQueue() : this(Comparer<T>.Default) { } public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default) { } public PriorityQueue(IComparer<T> comparer) { this.data = new List<T>(); this.comparer = comparer; } public PriorityQueue(int capacity, IComparer<T> comparer) { this.data = new List<T>(capacity); this.comparer = comparer; }[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public int Count => data.Count; public T Peek => data[0]; public void Add(T value) { data.Add(value); UpdateUp(data.Count - 1); } public T Dequeue() { var res = data[0]; data[0] = data[^1]; data.RemoveAt(data.Count - 1); UpdateDown(0); return res; } void UpdateUp(int i) { if (i > 0) { var p = (i - 1) >> 1; if (comparer.Compare(data[i], data[p]) < 0) { (data[p], data[i]) = (data[i], data[p]); UpdateUp(p); } } } void UpdateDown(int i) { var n = data.Count; var child = 2 * i + 1; if (child < n) { if (child != n - 1 && comparer.Compare(data[child], data[child + 1]) > 0) child++; if (comparer.Compare(data[i], data[child]) > 0) { (data[child], data[i]) = (data[i], data[child]); UpdateDown(child); } } }[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)] T[] Items => data.ToArray().Sort((a, b) => comparer.Compare(a, b)); }
    class PriorityQueue<TKey, TValue> : PriorityQueue<KeyValuePair<TKey, TValue>> { class KeyComparer : IComparer<KeyValuePair<TKey, TValue>> { public readonly IComparer<TKey> comparer; public KeyComparer(IComparer<TKey> comparer) { this.comparer = comparer; } public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => comparer.Compare(x.Key, y.Key); } public PriorityQueue() : this(Comparer<TKey>.Default) { } public PriorityQueue(int capacity) : this(capacity, Comparer<TKey>.Default) { } public PriorityQueue(IComparer<TKey> comparer) : base(new KeyComparer(comparer)) { } public PriorityQueue(int capacity, IComparer<TKey> comparer) : base(capacity, new KeyComparer(comparer)) { } public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value)); }

    public static int MaxFlow(this WNode[] graph, int from, int to)
    {
        var capacities = new Dictionary<(int from, int to), int>();
        for (int i = 0; i < graph.Length; i++)
            foreach (var next in graph[i].children)
                capacities.Add((i, next.to), next.value);

        var children = new HashSet<int>[graph.Length];
        for (int i = 0; i < children.Length; i++)
            children[i] = new HashSet<int>(graph[i].children.Select(c => c.to));
        var ret = 0;
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
                    if (routes[child] == null && capacities[route] > 0)
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

            var min = int.MaxValue;
            foreach (var route in routes[to])
                min = Math.Min(min, capacities[route]);

            ret += min;
            foreach (var route in routes[to])
            {
                capacities[route] -= min;

                var rev = (route.to, route.from);
                children[route.to].Add(route.from);
                capacities.TryGetValue(rev, out int v);
                capacities[rev] = v + min;
            }
        }
    }

    public static long[][] WarshallFloyd(this WNode[] graph)
    {
        var res = NewArray(graph.Length, graph.Length, 0L);
        for (var i = 0; i < graph.Length; i++)
        {
            for (var j = 0; j < graph.Length; j++)
            {
                res[i][j] = long.MaxValue / 2;
            }
            res[i][i] = 0;
            foreach (var next in graph[i].children)
                res[i][next.to] = next.value;
        }
        for (var k = 0; k < graph.Length; k++)
            for (var i = 0; i < graph.Length; i++)
                for (var j = 0; j < graph.Length; j++)
                    if (res[i][j] > res[i][k] + res[k][j])
                        res[i][j] = res[i][k] + res[k][j];
        return res;
    }
    public static long[] Dijkstra(this WNode[] graph, int start)
    {
        var res = NewArray(graph.Length, long.MaxValue / 2);
        res[start] = 0;

        var used = new bool[graph.Length];
        int count = 0;
        var remains = new PriorityQueue<long, int>();
        remains.Add(0, start);

        while (remains.Count > 0)
        {
            var first = remains.Dequeue();
            if (used[first.Value]) continue;
            used[first.Value] = true;
            if (++count >= graph.Length) break;
            foreach (var next in graph[first.Value].children)
            {
                var nextLength = first.Key + next.value;
                if (res[next.to] > nextLength)
                    remains.Add(res[next.to] = nextLength, next.to);
            }
        }
        return res;
    }
    public static long[] BellmanFord(this WNode[] graph, int start)
    {
        var res = NewArray(graph.Length, long.MaxValue / 2);
        res[start] = 0;

        for (int i = 1; i <= graph.Length; i++)
            foreach (var node in graph)
                foreach (var next in node.children)
                    if (res[next.to].UpdateMin(res[node.index] + next.value))
                        if (i == graph.Length)
                            throw new InvalidOperationException("負の閉路");
        return res;
    }


    public static WNode[] Prim(this WNode[] graph)
    {
        var sumi = new bool[graph.Length];
        var pq = new PriorityQueue<int, (int from, int to)>();
        var gb = new WGraphBuilder(graph.Length, false);
        sumi[0] = true;
        foreach (var next in graph[0].children)
            pq.Add(next.value, (0, next.to));
        for (int i = 1; i < graph.Length; i++)
        {
            var t = pq.Dequeue();
            if (sumi[t.Value.to]) { --i; continue; }
            sumi[t.Value.to] = true;
            gb.Add(t.Value.from, t.Value.to, t.Key);
            foreach (var next in graph[t.Value.to].children)
                if (!sumi[next.to])
                    pq.Add(next.value, (t.Value.to, next.to));
        }
        return gb.ToArray();
    }
}