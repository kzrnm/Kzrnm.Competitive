using System;
using System.Numerics;

namespace Kzrnm.Competitive
{
    public static class 最短経路BellmanFord
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をベルマン・フォード法で求める。負の長さにも使える</para>
        /// <para>計算量: O(|V|・|E|)</para>
        /// </summary>
        public static T[] BellmanFord<T, TEdge>(this IWGraph<T, TEdge> graph, int from)
            where T : IMultiplicativeIdentity<T, T>, IMinMaxValue<T>, IIncrementOperators<T>, IAdditionOperators<T, T, T>, IDivisionOperators<T, T, T>, IComparable<T>
            where TEdge : IWGraphEdge<T>
        {
            var two = T.MultiplicativeIdentity + T.MultiplicativeIdentity;
            var inf = T.MaxValue / two;
            return BellmanFord(graph, from, inf);
        }
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をベルマン・フォード法で求める。負の長さにも使える</para>
        /// <para>たどり着けないときは <paramref name="inf"/> を返す。</para>
        /// <para>制約: 最短経路長が <paramref name="inf"/> 未満である</para>
        /// <para>計算量: O(|V|・|E|)</para>
        /// </summary>
        public static T[] BellmanFord<T, TEdge>(this IWGraph<T, TEdge> graph, int from, T inf)
            where T : IIncrementOperators<T>, IAdditionOperators<T, T, T>, IComparable<T>
            where TEdge : IWGraphEdge<T>
        {
            var graphArr = graph.AsArray();
            var res = new T[graphArr.Length];
            Array.Fill(res, inf);
            res[from] = default;

            for (int i = 1; i <= graphArr.Length; i++)
                foreach (var node in graphArr)
                    foreach (var e in node.Children)
                    {
                        var to = e.To;
                        var x = res[node.Index] + e.Value;
                        if (res[to].CompareTo(x) > 0)
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
