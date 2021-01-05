using System;
using System.Collections.Generic;

namespace AtCoder
{
    public static class 最小全域木Kruskal
    {
        /// <summary>
        /// <para>最小全域木をKruskal法で求める。</para>
        /// <para>計算量: O(E log(E))</para>
        /// </summary>
        public static (int from, TEdge edge)[] Kruskal<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TNode : IWNode<T, TEdge, TOp>
            where TEdge : IWEdge<T>
        {
            var graphArr = graph.AsArray();
            var res = new List<(int from, TEdge edge)>(graph.Length - 1);
            var uf = new UnionFind(graphArr.Length);
            var edges = new List<(int from, TEdge edge)>();
            foreach (var node in graphArr)
                foreach (var e in node.Children)
                    edges.Add((node.Index, e));
            edges.Sort(Comparer<(int from, TEdge edge)>.Create((t1, t2) => default(TOp).Compare(t1.edge.Value, t2.edge.Value)));
            foreach (var (from, e) in edges.AsSpan())
                if (uf.Merge(from, e.To))
                    res.Add((from, e));
            return res.ToArray();
        }
    }
}
