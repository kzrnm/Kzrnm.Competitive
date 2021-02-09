using AtCoder.Internal;
using System;

namespace AtCoder
{
    public static class 最短経路BellmanFord
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をベルマン・フォード法で求める。負の長さにも使える</para>
        /// <para>計算量: O(|V|・|E|)</para>
        /// </summary>
        public static T[] BellmanFord<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int from)
            where T : struct
            where TOp : struct, INumOperator<T>
            where TNode : IWGraphNode<T, TEdge, TOp>
            where TEdge : IWEdge<T>
        {
            TOp op = default;
            var graphArr = graph.AsArray();
            var INF = op.Divide(op.MaxValue, op.Increment(op.MultiplyIdentity));
            var res = Global.NewArray(graphArr.Length, INF);
            res[from] = default;

            for (int i = 1; i <= graphArr.Length; i++)
                foreach (var node in graphArr)
                    foreach (var e in node.Children)
                    {
                        var x = op.Add(res[node.Index], e.Value);
                        if (op.GreaterThan(res[e.To], x))
                        {
                            res[e.To] = x;
                            if (i == graphArr.Length)
                                throw new InvalidOperationException("負の閉路");
                        }
                    }
            return res;
        }
    }
}
