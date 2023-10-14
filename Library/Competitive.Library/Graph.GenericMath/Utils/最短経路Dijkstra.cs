using System;
using System.Numerics;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 最短経路(Dijkstra)
    public static class 最短経路Dijkstra
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をダイクストラ法で求める。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( (|E| + |V|) log |V| )</para>
        /// </remarks>
        public static T[] Dijkstra<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph, int from)
            where T : IAdditionOperators<T, T, T>, IMinMaxValue<T>, IComparable<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var graphArr = graph.AsArray();
            var INF = T.MaxValue;
            var res = new T[graphArr.Length];
            Array.Fill(res, INF);
            res[from] = default;

            var used = new bool[graphArr.Length];
            int count = 0;
            var remains = new PriorityQueueDijkstra<T>(graphArr.Length);
            remains.Enqueue(default, from);

            while (remains.TryDequeue(out var len, out var ix))
            {
                if (used[ix]) continue;
                used[ix] = true;
                if (++count >= graphArr.Length) break;
                foreach (var e in graphArr[ix].Children)
                {
                    var to = e.To;
                    var nextLength = len + e.Value;
                    if (res[to].CompareTo(nextLength) > 0)
                        remains.Enqueue(res[to] = nextLength, to);
                }
            }
            return res;
        }
    }
}
