using AtCoder.Internal;

namespace AtCoder
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
            where TNode : IWNode<T, TEdge, TOp>
            where TEdge : IWEdge<T>
        {
            TOp op = default;
            var INF = op.Divide(op.MaxValue, op.Increment(op.MultiplyIdentity));
            var res = Global.NewArray(graph.Length, INF);
            res[from] = default;

            var used = new bool[graph.Length];
            int count = 0;
            var remains = new PriorityQueueOp<T, int, TOp>();
            remains.Add(default, from);

            while (remains.Count > 0)
            {
                var (len, ix) = remains.Dequeue();
                if (used[ix]) continue;
                used[ix] = true;
                if (++count >= graph.Length) break;
                foreach (var e in graph[ix].Children)
                {
                    var nextLength = op.Add(len, e.Value);
                    if (op.GreaterThan(res[e.To], nextLength))
                        remains.Add(res[e.To] = nextLength, e.To);
                }
            }
            return res;
        }
    }
}
