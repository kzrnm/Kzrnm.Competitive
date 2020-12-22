using AtCoder.Internal;
using System;

namespace AtCoder.Graph
{
    public static class 最短経路BellmanFord
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をベルマン・フォード法で求める。負の長さにも使える</para>
        /// <para>計算量: O(|V|・|E|)</para>
        /// </summary>
        public static T[] BellmanFord<T, TEdge, TOp>(this IWNode<T, TEdge, TOp>[] graph, int from)
            where TEdge : IWEdge<T>
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            TOp op = default;
            var INF = op.Divide(op.MaxValue, op.Increment(op.Increment(default)));
            var res = Global.NewArray(graph.Length, INF);
            res[from] = default;

            for (int i = 1; i <= graph.Length; i++)
                foreach (var node in graph)
                    foreach (var e in node.Children)
                    {
                        var x = op.Add(res[node.Index], e.Value);
                        if (op.GreaterThan(res[e.To], x))
                        {
                            res[e.To] = x;
                            if (i == graph.Length)
                                throw new InvalidOperationException("負の閉路");
                        }
                    }
            return res;
        }
    }
}
