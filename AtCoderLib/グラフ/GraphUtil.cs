using System;
using System.Collections.Generic;
using System.Linq;


static class GraphUtil
{
    public static int[] 強連結成分分解(this Node[] graph)
    {
        var sumi = new bool[graph.Length];
        Span<int> Dfs1(int index, Span<int> jun)
        {
            if (sumi[index])
                return jun;
            sumi[index] = true;
            foreach (var child in graph[index].children)
            {
                jun = Dfs1(child, jun);
            }
            jun[^1] = index;
            jun = jun[0..^1];
            return jun;
        }

        var jun = new int[graph.Length];
        var junsp = jun.AsSpan();
        for (int i = 0; i < graph.Length; i++)
            junsp = Dfs1(i, junsp);

        var res = NewArray(graph.Length, -1);
        bool Dfs2(int index, int group)
        {
            if (res[index] >= 0)
                return false;
            res[index] = group;
            foreach (var r in graph[index].roots)
                Dfs2(r, group);
            return true;
        }

        var g = 0;
        foreach (var i in jun)
            if (Dfs2(i, g))
                g++;
        return res;
    }

    public static int MaxFlow(this Node[] graph, int from, int to)
    {
        var capacities = new Dictionary<(int from, int to), int>();
        for (int i = 0; i < graph.Length; i++)
            foreach (var next in graph[i].children)
                capacities.Add((i, next), 1);

        var children = new HashSet<int>[graph.Length];
        for (int i = 0; i < children.Length; i++)
            children[i] = new HashSet<int>(graph[i].children);
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
    public static Node[] 最小全域木BFS(this Node[] graph)
    {
        var sumi = new bool[graph.Length];
        var gb = new GraphBuilder(graph.Length, false);
        var queue = new Queue<int>(graph.Length);
        queue.Enqueue(0);
        sumi[0] = true;
        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            sumi[cur] = true;
            foreach (var child in graph[cur].children)
            {
                if (!sumi[child])
                {
                    sumi[child] = true;
                    gb.Add(cur, child);
                    queue.Enqueue(child);
                }
            }
        }
        return gb.ToArray();
    }
}
