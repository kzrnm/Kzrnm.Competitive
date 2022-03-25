using AtCoder.Internal;
using AtCoder.Operators;
using System.Collections.Generic;
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
        public static (int from, TEdge edge)[] MinimumSpanningTreePrim<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int root = 0)
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var graphArr = graph.AsArray();
            var sumi = new bool[graphArr.Length];
            var pq = new PriorityQueueOp<TEdge, int, Comparer<T, TOp, TEdge>>(graphArr.Length);
            var res = new List<(int from, TEdge edge)>(graphArr.Length - 1);
            sumi[root] = true;
            foreach (var e in graphArr[root].Children)
                pq.Enqueue(e, root);
            var sumiCnt = 1;
            while (sumiCnt < sumi.Length && pq.TryDequeue(out var edge, out var from))
            {
                var to = edge.To;
                if (sumi[to]) continue;
                sumi[to] = true;
                ++sumiCnt;
                res.Add((from, edge));
                foreach (var e in graphArr[to].Children)
                    if (!sumi[e.To])
                        pq.Enqueue(e, to);
            }
            return res.ToArray();
        }
        private readonly struct Comparer<T, TOp, TEdge> : IComparer<TEdge>
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TEdge : IWGraphEdge<T>
        {
            [凾(256)]
            public int Compare(TEdge x, TEdge y) => new TOp().Compare(x.Value, y.Value);
        }
    }
}
