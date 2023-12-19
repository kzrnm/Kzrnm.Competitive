using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kzrnm.Competitive
{
    public static class 最小全域森Kruskal
    {
        /// <summary>
        /// <para>最小全域森をKruskal法で求めます。グラフが連結なら最小全域木となります。</para>
        /// <para>計算量: O(E log(E))</para>
        /// </summary>
        public static (int from, TEdge edge)[][] MinimumSpanningForestKruskal<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph)
            where T : IComparable<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var edges = new List<(int from, TEdge edge)>();
            foreach (var node in graph.AsArray())
                foreach (var e in node.Children)
                    edges.Add((node.Index, e));

            var es = edges.AsSpan();
            edges.Select(t => t.edge.Value).ToArray().AsSpan().Sort(es);
            return KruskalCore<TEdge>(graph.Length, edges.AsSpan());
        }

        /// <summary>
        /// <para>重みがないグラフの最小全域森をKruskal法で求めます。グラフが連結なら最小全域木となります。</para>
        /// <para>計算量: O(E α(E))</para>
        /// </summary>
        public static (int from, TEdge edge)[][] MinimumSpanningForestKruskal<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge
        {
            var edges = new List<(int from, TEdge edge)>();
            foreach (var node in graph.AsArray())
                foreach (var e in node.Children)
                    edges.Add((node.Index, e));
            return KruskalCore<TEdge>(graph.Length, edges.AsSpan());
        }

        /// <summary>
        /// <para>最小全域森をKruskal法で求めます。グラフが連結なら最小全域木となります。</para>
        /// </summary>
        /// <param name="n">グラフの頂点数</param>
        /// <param name="edges">ソート済みのグラフの辺のリスト</param>
        static (int from, TEdge edge)[][] KruskalCore<TEdge>(int n, ReadOnlySpan<(int from, TEdge edge)> edges)
           where TEdge : IGraphEdge
        {
            static List<(int from, TEdge edge)> Merge(List<(int from, TEdge edge)> a, List<(int from, TEdge edge)> b)
            {
                Debug.Assert(b == null || a?.Count >= b.Count);
                if (a != null && b != null)
                    a.AddRange(b);
                return a;
            }
            var uf = new UnionFind<List<(int from, TEdge edge)>>(new List<(int from, TEdge edge)>[n], Merge);
            foreach (var (from, e) in edges)
            {
                if (uf.Merge(from, e.To))
                    (uf.Data(from) ??= new()).Add((from, e));
            }
            var gg = uf.Groups();
            var res = new (int from, TEdge edge)[gg.Length][];
            for (int i = 0; i < res.Length; i++)
                res[i] = uf.Data(gg[i][0]).AsSpan().ToArray();

            return res;
        }
    }
}
