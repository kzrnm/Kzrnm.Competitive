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
            var pq = new PriorityQueueOp<(int from, TEdge edge), Comparer<T, TOp, TEdge>>();
            var res = new List<(int from, TEdge edge)>(graph.Length - 1);
            sumi[0] = true;
            foreach (var e in graph[0].Children)
                pq.Add((0, e));
            var sumiCnt = 1;
            while (sumiCnt < sumi.Length && pq.Count > 0)
            {
                var (from, edge) = pq.Dequeue();
                if (sumi[edge.To]) continue;
                sumi[edge.To] = true;
                ++sumiCnt;
                res.Add((from, edge));
                foreach (var e in graph[edge.To].Children)
                    if (!sumi[e.To])
                        pq.Add((edge.To, e));
            }
            return res.ToArray();
        }
        private readonly struct Comparer<T, TOp, TEdge> : IComparer<(int from, TEdge edge)>
            where TOp : struct, IAdditionOperator<T>, IComparer<T>
            where TEdge : IWEdge<T>
        {
            private static readonly TOp op = default;
            public int Compare((int from, TEdge edge) x, (int from, TEdge edge) y)
                => op.Compare(x.edge.Value, y.edge.Value);
        }
    }
}
