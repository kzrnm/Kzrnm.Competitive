using AtCoder.Internal;

namespace AtCoder.Graph
{
    public static class 最小全域木Prim
    {
        /// <summary>
        /// <para>最小全域木をPrim法で求める。</para>
        /// <para>計算量は O(E + V log(V))</para>
        /// </summary>
        public static WGraphBuilder<T, TOp> Prim<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where T : struct
            where TOp : struct, INumOperator<T>
            where TNode : IWNode<T, TEdge, TOp>
            where TEdge : IWEdge<T>
        {
            var sumi = new bool[graph.Length];
            var pq = new PriorityQueueOp<T, (int from, int to), TOp>();
            var gb = new WGraphBuilder<T, TOp>(graph.Length, false);
            sumi[0] = true;
            foreach (var e in graph[0].Children)
                pq.Add(e.Value, (0, e.To));
            for (int i = 1; i < graph.Length; i++)
            {
                var t = pq.Dequeue();
                if (sumi[t.Value.to]) { --i; continue; }
                sumi[t.Value.to] = true;
                gb.Add(t.Value.from, t.Value.to, t.Key);
                foreach (var e in graph[t.Value.to].Children)
                    if (!sumi[e.To])
                        pq.Add(e.Value, (t.Value.to, e.To));
            }
            return gb;
        }
    }
}
