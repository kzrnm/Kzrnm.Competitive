using System;
using System.Collections.Generic;
using System.Text;

static class 最小全域木
{
    public static WNode[] Kruskal(this WNode[] graph)
    {
        var gb = new WGraphBuilder(graph.Length, false);
        var uf = new UnionFind(graph.Length);
        var edges = new List<Tuple<int, int, int>>();
        foreach (var node in graph)
            foreach (var next in node.children)
                edges.Add(Tuple.Create(node.index, next.to, next.value));
        edges.Sort(Comparer<Tuple<int, int, int>>.Create((t1, t2) => t1.Item3.CompareTo(t2.Item3)));
        foreach (var e in edges)
        {
            if (!uf.IsSameSet(e.Item1, e.Item2))
            {
                uf.UnionSet(e.Item1, e.Item2);
                gb.Add(e.Item1, e.Item2, e.Item3);
            }
        }
        return gb.ToArray();
    }

    public static WNode[] Prim(this WNode[] graph)
    {
        var sumi = new bool[graph.Length];
        var pq = new PriorityQueue<int, Tuple<int, int>>();
        var gb = new WGraphBuilder(graph.Length, false);
        sumi[0] = true;
        foreach (var next in graph[0].children)
            pq.Add(next.value, Tuple.Create(0, next.to));
        for (int i = 1; i < graph.Length; i++)
        {
            var t = pq.Dequeue();
            if (sumi[t.Value.Item2]) { --i; continue; }
            sumi[t.Value.Item2] = true;
            gb.Add(t.Value.Item1, t.Value.Item2, t.Key);
            foreach (var next in graph[t.Value.Item2].children)
                if (!sumi[next.to])
                    pq.Add(next.value, Tuple.Create(t.Value.Item2, next.to));
        }
        return gb.ToArray();
    }
}
