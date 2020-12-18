using System.Collections.Generic;

namespace AtCoder.Graph
{
    public static class 最小全域木
    {
        public static WNode<int>[] Kruskal(this WNode<int>[] graph)
            => Kruskal<int, IntOperator>(graph);
        public static WNode<long>[] Kruskal(this WNode<long>[] graph)
            => Kruskal<long, LongOperator>(graph);
        public static WNode<T>[] Kruskal<T, TOp>(this WNode<T>[] graph)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            var gb = new WGraphBuilder<T, TOp>(graph.Length, false);
            var uf = new UnionFind(graph.Length);
            var edges = new List<(int from, int to, T value)>();
            foreach (var node in graph)
                foreach (var next in node.children)
                    edges.Add((node.index, next.to, next.value));
            edges.Sort(Comparer<(int from, int to, T value)>.Create((t1, t2) => default(TOp).Compare(t1.value, t2.value)));
            foreach (var (from, to, value) in edges.AsSpan())
            {
                if (!uf.Same(from, to))
                {
                    uf.Merge(from, to);
                    gb.Add(from, to, value);
                }
            }
            return gb.ToArray();
        }


        public static WNode<int>[] Prim(this WNode<int>[] graph)
            => Prim<int, IntOperator>(graph);
        public static WNode<long>[] Prim(this WNode<long>[] graph)
            => Prim<long, LongOperator>(graph);
        public static WNode<T>[] Prim<T, TOp>(this WNode<T>[] graph)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            var sumi = new bool[graph.Length];
            var pq = new PriorityQueue<T, (int from, int to)>();
            var gb = new WGraphBuilder<T, TOp>(graph.Length, false);
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
}