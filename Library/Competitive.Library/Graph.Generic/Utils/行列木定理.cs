using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class 行列木定理
    {
        /// <summary>
        /// 行列木定理から <paramref name="graph"/> の全域木の個数を計算するためのラプラシアン行列を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="graph"/> は無向グラフ</para>
        /// </remarks>
        [凾(256)]
        public static LaplacianWrapper MatrixTreeTheorem<TEdge>(this IGraph<TEdge> graph)
            where TEdge : IGraphEdge
        {
            if (graph.Length == 0) return new(new(Array.Empty<int>(), 0, 0));
            Contract.Assert(!graph[0].IsDirected, "graph は無向グラフである必要があります");
            return new(graph.Laplacian());
        }

        public readonly struct LaplacianWrapper
        {
            readonly ArrayMatrix<int> Matrix;
            public LaplacianWrapper(ArrayMatrix<int> m) { Matrix = m; }

            /// <summary>
            /// 全域木の個数を返します。
            /// </summary>
            public T Calc<T>() where T : IModInt<T>
            {
                var orig = Matrix._v;
                var n = Matrix.Height - 1;
                var a = new T[n][];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = new T[n];
                    ref var head = ref a[i][0];
                    var src = orig.AsSpan(i * (n + 1), n);
                    for (int j = 0; j < src.Length; j++)
                    {
                        Unsafe.Add(ref head, j) = T.CreateTruncating(src[j]);
                    }
                }
                return ArrayMatrixLogic<T>.DeterminantImpl(a);
            }
        }
    }
}
