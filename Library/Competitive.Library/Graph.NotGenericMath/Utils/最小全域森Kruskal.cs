using AtCoder;
using AtCoder.Operators;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 最小全域森(Kruskal)
    public static class 最小全域森Kruskal
    {
        /// <summary>
        /// <para>最小全域森をKruskal法で求める。グラフが連結なら最小全域木となる。</para>
        /// <para>計算量: O(E log(E))</para>
        /// </summary>
        public static (int from, TEdge edge)[][] Kruskal<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var graphArr = graph.AsArray();
            var uf = new Dsu(graphArr.Length);
            var edges = new List<(int from, TEdge edge)>();
            foreach (var node in graphArr)
                foreach (var e in node.Children)
                    edges.Add((node.Index, e));
            edges.Sort(Comparer<(int from, TEdge edge)>.Create((t1, t2) => new TOp().Compare(t1.edge.Value, t2.edge.Value)));
            var gr = new List<(int from, TEdge edge)>[graph.Length];
            foreach (var (from, e) in edges.AsSpan())
            {
                var f = uf.Leader(from);
                var t = uf.Leader(e.To);
                var grf = gr[f];
                var grt = gr[t];
                if (f == t) continue;
                else if (grf == null && grt == null)
                    gr[uf.Merge(f, t)] = new List<(int from, TEdge edge)> { (from, e) };
                else if (grt == null)
                {
                    grf.Add((from, e));
                    gr[uf.Merge(f, t)] = grf;
                }
                else if (grf == null)
                {
                    grt.Add((from, e));
                    gr[uf.Merge(f, t)] = grt;
                }
                else
                {
                    // 多い方に統合する。 grf が多い方だとする
                    if (grf.Count < grt.Count)
                        (grf, grt) = (grt, grf);
                    foreach (var tt in grt.AsSpan())
                        grf.Add(tt);
                    grf.Add((from, e));
                    gr[uf.Merge(f, t)] = grf;
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
        public static (int from, TEdge edge)[][] MinimumSpanningForestKruskal<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge
        {
            var graphArr = graph.AsArray();
            var uf = new Dsu(graphArr.Length);
            var edges = new List<(int from, TEdge edge)>();
            foreach (var node in graphArr)
                foreach (var e in node.Children)
                    edges.Add((node.Index, e));
            var gr = new List<(int from, TEdge edge)>[graph.Length];
            foreach (var (from, e) in edges.AsSpan())
            {
                var f = uf.Leader(from);
                var t = uf.Leader(e.To);
                var grf = gr[f];
                var grt = gr[t];
                if (f == t) continue;
                else if (grf == null && grt == null)
                    gr[uf.Merge(f, t)] = new List<(int from, TEdge edge)> { (from, e) };
                else if (grt == null)
                {
                    grf.Add((from, e));
                    gr[uf.Merge(f, t)] = grf;
                }
                else if (grf == null)
                {
                    grt.Add((from, e));
                    gr[uf.Merge(f, t)] = grt;
                }
                else
                {
                    // 多い方に統合する。 grf が多い方だとする
                    if (grf.Count < grt.Count)
                        (grf, grt) = (grt, grf);
                    foreach (var tt in grt.AsSpan())
                        grf.Add(tt);
                    grf.Add((from, e));
                    gr[uf.Merge(f, t)] = grf;
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
