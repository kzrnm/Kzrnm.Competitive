using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class 最小全域木Prim
    {
        /// <summary>
        /// <para>最小全域木をPrim法で求める。</para>
        /// <para><paramref name="root"/>を根とする木を構築する。</para>
        /// <para>計算量は O(E + V log(V))</para>
        /// </summary>
        public static MstResult<T, TEdge> MinimumSpanningTreePrim<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph, int root = 0)
            where T : IComparable<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            Contract.Assert(!graph[0].IsDirected, "有向グラフでは求められません");

            var graphArr = graph.AsArray();
            var sumi = new bool[graphArr.Length];
            var pq = new PriorityQueueOp<TEdge, int, Comparer<T, TEdge>>(graphArr.Length);
            sumi[root] = true;
            foreach (var e in graphArr[root].Children)
                pq.Enqueue(e, root);
            var sumiCnt = 1;
            var b = new MstBuilder<T, TEdge>(graphArr.Length - 1);
            while (sumiCnt < sumi.Length && pq.TryDequeue(out var edge, out var from))
            {
                var to = edge.To;
                if (sumi[to]) continue;
                sumi[to] = true;
                ++sumiCnt;
                b.Add((from, edge));
                foreach (var e in graphArr[to].Children)
                    if (!sumi[e.To])
                        pq.Enqueue(e, to);
            }
            return b.Build();
        }
        private readonly struct Comparer<T, TEdge> : IComparer<TEdge>
            where T : IComparable<T>
            where TEdge : IWGraphEdge<T>
        {
            [凾(256)]
            public int Compare(TEdge x, TEdge y) => x.Value.CompareTo(y.Value);
        }
    }
}
