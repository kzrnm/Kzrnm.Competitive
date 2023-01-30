using System;
using System.Numerics;

namespace Kzrnm.Competitive
{
    public static class 最短経路WarshallFloyd
    {
        /// <summary>
        /// <para>各頂点間の最短経路長をワーシャルフロイド法で求める。</para>
        /// <para>計算量: O(|V|^3)</para>
        /// </summary>
        public static T[][] WarshallFloyd<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph)
            where T : IMultiplicativeIdentity<T, T>, IMinMaxValue<T>, IIncrementOperators<T>, IAdditionOperators<T, T, T>, IDivisionOperators<T, T, T>, IComparable<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var two = T.MultiplicativeIdentity + T.MultiplicativeIdentity;
            var inf = T.MaxValue / two;
            return WarshallFloyd(graph, inf);
        }
        /// <summary>
        /// <para>各頂点間の最短経路長をワーシャルフロイド法で求める。</para>
        /// <para>たどり着けないときは <paramref name="inf"/> を返す。</para>
        /// <para>制約: 最短経路長が <paramref name="inf"/> 未満である</para>
        /// <para>計算量: O(|V|^3)</para>
        /// </summary>
        public static T[][] WarshallFloyd<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph, T inf)
            where T : IAdditionOperators<T, T, T>, IComparable<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var graphArr = graph.AsArray();
            var res = new T[graphArr.Length][];
            for (var i = 0; i < graphArr.Length; i++)
            {
                res[i] = new T[graphArr.Length];
                Array.Fill(res[i], inf);
                res[i][i] = default;
                foreach (var e in graphArr[i].Children)
                    res[i][e.To] = e.Value;
            }
            for (var k = 0; k < graphArr.Length; k++)
                for (var i = 0; i < graphArr.Length; i++)
                    for (var j = 0; j < graphArr.Length; j++)
                    {
                        var x = res[i][k] + res[k][j];
                        if (res[i][j].CompareTo(x) > 0)
                            res[i][j] = x;
                    }
            return res;
        }
    }
}
