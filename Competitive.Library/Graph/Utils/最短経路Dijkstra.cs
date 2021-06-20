using AtCoder;
using AtCoder.Internal;

namespace Kzrnm.Competitive
{
    public static class 最短経路Dijkstra
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をダイクストラ法で求める。</para>
        /// <para>計算量: O( (|E| + |V|) log |V| )</para>
        /// </summary>
        public static T[] Dijkstra<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int from)
            where T : struct
            where TOp : struct, INumOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var graphArr = graph.AsArray();
            var INF = op.MaxValue;
            var res = Global.NewArray(graphArr.Length, INF);
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
                    var nextLength = op.Add(len, e.Value);
                    if (op.GreaterThan(res[e.To], nextLength))
                        remains.Enqueue(res[e.To] = nextLength, e.To);
                }
            }
            return res;
        }
    }
}
