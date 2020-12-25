using System.Collections.Generic;

namespace AtCoder.Graph
{
    public static class 最小全域木Kruskal
    {
        /// <summary>
        /// <para>最小全域木をKruskal法で求める。多分Prim法の方が良い。</para>
        /// <para>計算量: O(E log(E))</para>
        /// </summary>
        public static WGraphBuilder<T, TOp> Kruskal<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where T : struct
            where TOp : struct, INumOperator<T>
            where TNode : IWNode<T, TEdge, TOp>
            where TEdge : IWEdge<T>
        {
            var graphArr = graph.AsArray();
            var gb = new WGraphBuilder<T, TOp>(graphArr.Length, false);
            var uf = new UnionFind(graphArr.Length);
            var edges = new List<(int from, int to, T value)>();
            foreach (var node in graphArr)
                foreach (var e in node.Children)
                    edges.Add((node.Index, e.To, e.Value));
            edges.Sort(Comparer<(int from, int to, T value)>.Create((t1, t2) => default(TOp).Compare(t1.value, t2.value)));
            foreach (var (from, to, value) in edges.AsSpan())
            {
                if (uf.Merge(from, to))
                {
                    gb.Add(from, to, value);
                }
            }
            return gb;
        }
    }
}
