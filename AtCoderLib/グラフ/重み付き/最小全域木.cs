using System;
using System.Collections.Generic;
using System.Text;

static class 最小全域木
{
    public static WNode[] Kruskal(this WNode[] graph)
    {
        var gb = new WGraphBuilder(graph.Length, false);
        var uf = new UnionFind(graph.Length);
        var edges = new List<(int from, int to, int value)>();
        foreach (var node in graph)
            foreach (var next in node.children)
                edges.Add((node.index, next.to, next.value));
        edges.Sort(Comparer<(int from, int to, int value)>.Create((t1, t2) => t1.value.CompareTo(t2.value)));
        foreach (var (from, to, value) in edges)
        {
            if (!uf.IsSameRoot(from, to))
            {
                uf.Merge(from, to);
                gb.Add(from, to, value);
            }
        }
        return gb.ToArray();
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
