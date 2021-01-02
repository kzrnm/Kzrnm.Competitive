namespace AtCoder
{
    public static class 最短経路WarshallFloyd
    {
        /// <summary>
        /// <para>各頂点間の最短経路長をワーシャルフロイド法で求める</para>
        /// <para>計算量: O(|V|^3)</para>
        /// </summary>
        public static T[][] WarshallFloyd<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
        where T : struct
        where TOp : struct, INumOperator<T>
        where TNode : IWNode<T, TEdge, TOp>
        where TEdge : IWEdge<T>
        {
            TOp op = default;
            var INF = op.Divide(op.MaxValue, op.Increment(op.Increment(default)));
            var res = Global.NewArray(graph.Length, graph.Length, INF);
            for (var i = 0; i < graph.Length; i++)
            {
                res[i][i] = default;
                foreach (var e in graph[i].Children)
                    res[i][e.To] = e.Value;
            }
            for (var k = 0; k < graph.Length; k++)
                for (var i = 0; i < graph.Length; i++)
                    for (var j = 0; j < graph.Length; j++)
                    {
                        var x = op.Add(res[i][k], res[k][j]);
                        if (op.GreaterThan(res[i][j], x))
                            res[i][j] = x;
                    }
            return res;
        }
    }
}
