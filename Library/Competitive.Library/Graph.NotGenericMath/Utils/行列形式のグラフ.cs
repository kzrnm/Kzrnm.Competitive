using AtCoder;
using AtCoder.Operators;
using System;

namespace Kzrnm.Competitive
{
    public static class 行列形式のグラフ
    {
        /// <summary>
        /// グラフの隣接行列を返します。
        /// </summary>
        public static ArrayMatrix<int, IntOperator> Adjacency<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge
        {
            var g = graph.AsArray();
            var N = g.Length;
            var arr = new int[N * N];
            for (int i = 0; i < g.Length; i++)
            {
                foreach (var e in g[i].Children)
                {
                    arr[i * N + e.To] = 1;
                }
            }
            return new ArrayMatrix<int, IntOperator>(arr, N, N);
        }

        /// <summary>
        /// グラフの隣接行列を返します。
        /// </summary>
        public static ArrayMatrix<T, TOp> Adjacency<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph)
            where T : IEquatable<T>
            where TOp : struct, IArithmeticOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var g = graph.AsArray();
            var N = g.Length;
            var arr = new T[N * N];
            for (int i = 0; i < g.Length; i++)
            {
                foreach (var e in g[i].Children)
                {
                    arr[i * N + e.To] = e.Value;
                }
            }
            return new ArrayMatrix<T, TOp>(arr, N, N);
        }

        /// <summary>
        /// グラフのラプラシアン行列を返します。
        /// </summary>
        public static ArrayMatrix<int, IntOperator> Laplacian<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge
        {
            var g = graph.AsArray();
            var N = g.Length;
            var arr = new int[N * N];
            for (int i = 0; i < g.Length; i++)
            {
                arr[i * N + i] = g[i].Children.Length;
                foreach (var e in g[i].Children)
                {
                    arr[i * N + e.To] = -1;
                }
            }
            return new ArrayMatrix<int, IntOperator>(arr, N, N);
        }
    }
}
