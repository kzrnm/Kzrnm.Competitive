using AtCoder.Operators;
using System;

namespace Kzrnm.Competitive
{
    public static class 最短経路BellmanFord
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をベルマン・フォード法で求める。負の長さにも使える</para>
        /// <para>計算量: O(|V|・|E|)</para>
        /// </summary>
        public static T[] BellmanFord<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int from)
            where TOp : struct, INumOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var inf = op.Divide(op.MaxValue, op.Increment(op.MultiplyIdentity));
            return BellmanFord(graph, from, inf);
        }
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をベルマン・フォード法で求める。負の長さにも使える</para>
        /// <para>たどり着けないときは <paramref name="inf"/> を返す。</para>
        /// <para>制約: 最短経路長が <paramref name="inf"/> 未満である</para>
        /// <para>計算量: O(|V|・|E|)</para>
        /// </summary>
        public static T[] BellmanFord<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int from, T inf)
            where TOp : struct, ICompareOperator<T>, IAdditionOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var graphArr = graph.AsArray();
            var res = new T[graphArr.Length];
            Array.Fill(res, inf);
            res[from] = default;

            for (int i = 1; i <= graphArr.Length; i++)
                foreach (var node in graphArr)
                    foreach (var e in node.Children)
                    {
                        var to = e.To;
                        var x = op.Add(res[node.Index], e.Value);
                        if (op.GreaterThan(res[to], x))
                        {
                            res[to] = x;
                            if (i == graphArr.Length)
                                throw new InvalidOperationException("負の閉路");
                        }
                    }
            return res;
        }
    }
}
