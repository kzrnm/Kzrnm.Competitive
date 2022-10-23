using AtCoder.Operators;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class 最短経路Dijkstra
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をダイクストラ法で求める。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( (|E| + |V|) log |V| )</para>
        /// </remarks>
        public static T[] Dijkstra<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int from)
            where T : struct
            where TOp : struct, IAdditionOperator<T>, IMinMaxValue<T>, IComparer<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var graphArr = graph.AsArray();
            var INF = op.MaxValue;
            var res = new T[graphArr.Length];
            Array.Fill(res, INF);
            res[from] = default;

            var used = new bool[graphArr.Length];
            int count = 0;
            var remains = new PriorityQueueDijkstra<T, TOp>(graphArr.Length);
            remains.Enqueue(default, from);

            while (remains.TryDequeue(out var len, out var ix))
            {
                if (used[ix]) continue;
                used[ix] = true;
                if (++count >= graphArr.Length) break;
                foreach (var e in graphArr[ix].Children)
                {
                    var to = e.To;
                    var nextLength = op.Add(len, e.Value);
                    if (op.Compare(res[to], nextLength) > 0)
                        remains.Enqueue(res[to] = nextLength, to);
                }
            }
            return res;
        }
    }
}
