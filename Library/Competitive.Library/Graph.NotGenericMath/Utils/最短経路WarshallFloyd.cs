using AtCoder.Operators;
using System;

namespace Kzrnm.Competitive
{
    public static class 最短経路WarshallFloyd
    {
        /// <summary>
        /// <para>各頂点間の最短経路長をワーシャルフロイド法で求める。</para>
        /// <para>計算量: O(|V|^3)</para>
        /// </summary>
        public static T[][] WarshallFloyd<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where TOp : struct, INumOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var inf = op.Divide(op.MaxValue, op.Increment(op.MultiplyIdentity));
            return WarshallFloyd(graph, inf);
        }
        /// <summary>
        /// <para>各頂点間の最短経路長をワーシャルフロイド法で求める。</para>
        /// <para>たどり着けないときは <paramref name="inf"/> を返す。</para>
        /// <para>制約: 最短経路長が <paramref name="inf"/> 未満である</para>
        /// <para>計算量: O(|V|^3)</para>
        /// </summary>
        public static T[][] WarshallFloyd<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, T inf)
            where TOp : struct, IAdditionOperator<T>, ICompareOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var graphArr = graph.AsArray();
            var res = new T[graphArr.Length][];
            for (var i = 0; i < graphArr.Length; i++)
            {
                res[i] = new T[graphArr.Length];
                Array.Fill(res[i], inf);
                foreach (var e in graphArr[i].Children)
                    res[i][e.To] = e.Value;
                res[i][i] = default;
            }
            for (var k = 0; k < graphArr.Length; k++)
                for (var i = 0; i < graphArr.Length; i++)
                    for (var j = 0; j < graphArr.Length; j++)
                    {
                        var x = op.Add(res[i][k], res[k][j]);
                        if (op.GreaterThan(res[i][j], x))
                            res[i][j] = x;
                    }
            return res;
        }
    }
}
