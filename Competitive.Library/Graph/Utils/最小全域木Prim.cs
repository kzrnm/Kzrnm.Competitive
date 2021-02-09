using AtCoder;
using AtCoder.Internal;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class 最小全域木Prim
    {
        /// <summary>
        /// <para>最小全域木をPrim法で求める。</para>
        /// <para>計算量は O(E + V log(V))</para>
        /// </summary>
        public static (int from, TEdge edge)[] Prim<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TNode : IWGraphNode<T, TEdge, TOp>
            where TEdge : IWEdge<T>
        {
            var sumi = new bool[graph.Length];
            var pq = new PriorityQueueOp<TEdge, int, Comparer<T, TOp, TEdge>>();
            var res = new List<(int from, TEdge edge)>(graph.Length - 1);
            sumi[0] = true;
            foreach (var e in graph[0].Children)
                pq.Add(e, 0);
            var sumiCnt = 1;
            while (sumiCnt < sumi.Length && pq.TryDequeue(out var edge, out var from))
            {
                if (sumi[edge.To]) continue;
                sumi[edge.To] = true;
                ++sumiCnt;
                res.Add((from, edge));
                foreach (var e in graph[edge.To].Children)
                    if (!sumi[e.To])
                        pq.Add(e, edge.To);
            }
            return res.ToArray();
        }
        private readonly struct Comparer<T, TOp, TEdge> : IComparer<TEdge>
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TEdge : IWEdge<T>
        {
            private static readonly TOp op = default;
            public int Compare(TEdge x, TEdge y) => op.Compare(x.Value, y.Value);
        }
    }
}
