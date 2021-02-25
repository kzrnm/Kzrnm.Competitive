using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class 最小全域木Kruskal
    {
        /// <summary>
        /// <para>最小全域森をKruskal法で求める。グラフが連結なら最小全域木となる。</para>
        /// <para>計算量: O(E log(E))</para>
        /// </summary>
        public static (int from, TEdge edge)[][] Kruskal<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWEdge<T>
        {
            var graphArr = graph.AsArray();
            var uf = new DSU(graphArr.Length);
            var edges = new SimpleList<(int from, TEdge edge)>();
            foreach (var node in graphArr)
                foreach (var e in node.Children)
                    edges.Add((node.Index, e));
            edges.Sort(Comparer<(int from, TEdge edge)>.Create((t1, t2) => default(TOp).Compare(t1.edge.Value, t2.edge.Value)));
            var gr = new SimpleList<(int from, TEdge edge)>[graph.Length];
            foreach (var (from, e) in edges.AsSpan())
            {
                var f = uf.Leader(from);
                var t = uf.Leader(e.To);
                if (f == t) continue;
                else if (gr[f] == null && gr[t] == null)
                    gr[uf.Merge(f, t)] = new SimpleList<(int from, TEdge edge)> { (from, e) };
                else if (gr[t] == null)
                {
                    gr[f].Add((from, e));
                    gr[uf.Merge(f, t)] = gr[f];
                }
                else if (gr[f] == null)
                {
                    gr[t].Add((from, e));
                    gr[uf.Merge(f, t)] = gr[t];
                }
                else
                {
                    var m = uf.Merge(f, t);
                    if (t == m) t = f;
                    foreach (var tt in gr[t].AsSpan())
                        gr[m].Add(tt);
                    gr[m].Add((from, e));
                }
            }
            var gg = uf.Groups();
            var res = new (int from, TEdge edge)[gg.Length][];
            for (int i = 0; i < res.Length; i++)
                res[i] = gr[uf.Leader(gg[i][0])]?.ToArray() ?? Array.Empty<(int from, TEdge edge)>();
            return res;
        }

        /// <summary>
        /// <para>最小全域森をKruskal法で求める。グラフが連結なら最小全域木となる。</para>
        /// <para>計算量: O(E log(E))</para>
        /// </summary>
        public static (int from, TEdge edge)[][] Kruskal<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : IGraphNode<TEdge>
            where TEdge : IEdge
        {
            var graphArr = graph.AsArray();
            var uf = new DSU(graphArr.Length);
            var edges = new SimpleList<(int from, TEdge edge)>();
            foreach (var node in graphArr)
                foreach (var e in node.Children)
                    edges.Add((node.Index, e));
            var gr = new SimpleList<(int from, TEdge edge)>[graph.Length];
            foreach (var (from, e) in edges.AsSpan())
            {
                var f = uf.Leader(from);
                var t = uf.Leader(e.To);
                if (f == t) continue;
                else if (gr[f] == null && gr[t] == null)
                    gr[uf.Merge(f, t)] = new SimpleList<(int from, TEdge edge)> { (from, e) };
                else if (gr[t] == null)
                {
                    gr[f].Add((from, e));
                    gr[uf.Merge(f, t)] = gr[f];
                }
                else if (gr[f] == null)
                {
                    gr[t].Add((from, e));
                    gr[uf.Merge(f, t)] = gr[t];
                }
                else
                {
                    var m = uf.Merge(f, t);
                    if (t == m) t = f;
                    foreach (var tt in gr[t].AsSpan())
                        gr[m].Add(tt);
                    gr[m].Add((from, e));
                }
            }
            var gg = uf.Groups();
            var res = new (int from, TEdge edge)[gg.Length][];
            for (int i = 0; i < res.Length; i++)
                res[i] = gr[uf.Leader(gg[i][0])]?.ToArray() ?? Array.Empty<(int from, TEdge edge)>();
            return res;
        }
    }
}
