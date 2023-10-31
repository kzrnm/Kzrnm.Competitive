using System.Numerics;

namespace Kzrnm.Competitive
{
    public static class 行列形式のグラフ
    {
        /// <summary>
        /// グラフの隣接行列を返します。
        /// </summary>
        public static ArrayMatrix<int> Adjacency<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
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
            return new(arr, N, N);
        }

        /// <summary>
        /// グラフの隣接行列を返します。
        /// </summary>
        public static ArrayMatrix<T> Adjacency<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph)
            where T : INumberBase<T>
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
            return new(arr, N, N);
        }

        /// <summary>
        /// グラフのラプラシアン行列を返します。
        /// </summary>
        public static ArrayMatrix<int> Laplacian<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
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
                    --arr[i * N + e.To];
                }
            }
            return new(arr, N, N);
        }
    }
}
