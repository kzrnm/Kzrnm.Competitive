using System;
using System.Collections.Generic;
using static AtCoderProject.Global;

static class GraphUtil
{
    public static int[] 強連結成分分解(this Node[] graph)
    {
        int j = graph.Length;
        var jun = new int[graph.Length];
        var sumi = new bool[graph.Length];
        Action<int> dfs1 = null;
        dfs1 = index =>
        {
            if (sumi[index])
                return;
            sumi[index] = true;
            foreach (var child in graph[index].children)
                dfs1(child);
            jun[--j] = index;
        };
        for (int i = 0; i < graph.Length; i++)
            dfs1(i);

        var res = NewArray(graph.Length, -1);
        Func<int, int, bool> dfs2 = null;
        dfs2 = (index, group) =>
        {
            if (res[index] >= 0)
                return false;
            res[index] = group;
            foreach (var r in graph[index].roots)
                dfs2(r, group);
            return true;
        };

        var g = 0;
        foreach (var i in jun)
            if (dfs2(i, g))
                g++;
        return res;
    }
    public static int MaxFlow(this Node[] graph, int from, int to)
    {
        var capacities = new Dictionary<Tuple<int, int>, int>();
        for (int i = 0; i < graph.Length; i++)
            foreach (var next in graph[i].children)
                capacities.Add(Tuple.Create(i, next), 1);

        var children = new HashSet<int>[graph.Length];
        for (int i = 0; i < children.Length; i++)
            children[i] = new HashSet<int>(graph[i].children);
        var ret = 0;
        while (true)
        {
            var routes = new Tuple<int, int>[children.Length][];
            var queue = new Queue<int>(children.Length);
            routes[from] = Array.Empty<Tuple<int, int>>();
            queue.Enqueue(from);
            while (queue.Count > 0 && routes[to] == null)
            {
                var cur = queue.Dequeue();
                foreach (var child in children[cur])
                {
                    var route = Tuple.Create(cur, child);
                    if (routes[child] == null && capacities[route] > 0)
                    {
                        routes[child] = new Tuple<int, int>[routes[cur].Length + 1];
                        routes[cur].CopyTo(routes[child], 0);
                        routes[child][routes[child].Length - 1] = route;
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

                var rev = Tuple.Create(route.Item2, route.Item1);
                children[route.Item2].Add(route.Item1);
                int v;
                capacities.TryGetValue(rev, out v);
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
