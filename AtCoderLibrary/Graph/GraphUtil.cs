using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using static AtCoder.Global;

namespace AtCoder.Graph
{
    public static class GraphUtil
    {
        /// <summary>
        /// オイラー路を求める
        /// </summary>
        public static Span<int> EulerianTrail(this Node[] graph, int from)
            => EulerianTrail(graph.Select(n => new Queue<int>(n.children)).ToArray(), from, graph[from].IsDirected);
        private static Span<int> EulerianTrail(Queue<int>[] graph, int from, bool isDirected)
        {
            static (int, int) ToTuple(int v1, int v2) => (v1 > v2) ? (v2, v1) : (v1, v2);
            var cnts = new Dictionary<(int, int), int>();
            var res = new List<int>();
            var idx = new Stack<int>();
            idx.Push(from);
        Dfs:
            while (idx.Count > 0)
            {
                from = idx.Peek();
                while (graph[from].Count > 0)
                {
                    int to = graph[from].Dequeue();
                    if (!isDirected && from != to)
                    {
                        var tup = ToTuple(from, to);
                        var cnt = cnts.Get(tup);
                        if (cnt > 0)
                        {
                            cnts[tup] = cnt - 1;
                            continue;
                        }
                        cnts[tup] = 1;
                    }
                    idx.Push(to);
                    goto Dfs;
                }
                res.Add(idx.Pop());
            }

            res.Reverse();
            return res.AsSpan();
        }

        public static int[] 強連結成分分解(this Node[] graph)
        {
            var sumi = new bool[graph.Length];
            int[] Dfs1()
            {
                var jun = new int[graph.Length];
                var cur = graph.Length;
                var idx = new Stack<int>(graph.Length);
                var idx2 = new Stack<int>(graph.Length);

                for (int i = 0; i < graph.Length; i++)
                {
                    if (sumi[i])
                        continue;
                    idx.Push(i);
                    while (idx.Count > 0)
                    {
                        int index = idx.Pop();
                        if (sumi[index])
                            continue;
                        sumi[index] = true;
                        idx2.Push(index);
                        foreach (var child in graph[index].children)
                            idx.Push(child);
                    }
                    while (idx2.Count > 0)
                    {
                        int index = idx2.Pop();
                        jun[--cur] = index;
                    }
                }
                return jun;
            }

            var jun = Dfs1();

            var res = NewArray(graph.Length, -1);
            var idx = new Stack<int>(graph.Length);
            var g = 0;
            foreach (var i in jun)
            {
                if (res[i] >= 0)
                    continue;
                idx.Push(i);
                while (idx.Count > 0)
                {
                    int index = idx.Pop();
                    if (res[index] >= 0)
                        continue;
                    res[index] = g;
                    foreach (var r in graph[index].roots)
                        idx.Push(r);
                }
                ++g;
            }
            return res;
        }

        public static (int l, int r)[] EulerianTour(this TreeNode[] tree)
        {
            var root = 0;
            while (tree[root].root >= 0) root = tree[root].root;

            var cnt = 0;
            var res = new (int l, int r)[tree.Length];

            var idx = new Stack<(int index, int ci)>(tree.Length);
            idx.Push((root, 0));
            while (idx.Count > 0)
            {
                var (index, ci) = idx.Pop();
                if (ci == 0)
                    res[index].l = cnt++;

                if (ci < tree[index].children.Length)
                {
                    idx.Push((index, ci + 1));
                    idx.Push((tree[index].children[ci], 0));
                }
                else
                    res[index].r = cnt++;
            }

            /* 再帰版
            void Dfs(int index)
            {
                res[index].l = cnt++;
                foreach (var ch in tree[index].children)
                    Dfs(ch);
                res[index].r = cnt++;
            }
            Dfs(root);
            */
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

    public static class ShortestPathBFS
    {
        public static int[] BFS(this Node[] graph, int from)
        {
            var res = NewArray(graph.Length, int.MaxValue);
            var queue = new Queue<int>();
            queue.Enqueue(from);
            res[from] = 0;
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var child in graph[cur].children)
                    if (res[child].UpdateMin(res[cur] + 1))
                        queue.Enqueue(child);
            }
            return res;
        }
    }
}