using System;
using System.Collections.Generic;
using System.Linq;


static class WGraphUtil
{
    public static int MaxFlow(this WNode[] graph, int from, int to)
    {
        var capacities = new Dictionary<Tuple<int, int>, int>();
        for (int i = 0; i < graph.Length; i++)
            foreach (var next in graph[i].children)
                capacities.Add(Tuple.Create(i, next.to), next.value);

        var children = new HashSet<int>[graph.Length];
        for (int i = 0; i < children.Length; i++)
            children[i] = new HashSet<int>(graph[i].children.Select(c => c.to));
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
}
