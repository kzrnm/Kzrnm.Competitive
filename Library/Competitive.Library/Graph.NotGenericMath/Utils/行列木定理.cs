using AtCoder;
using AtCoder.Internal;
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
        public static LaplacianWrapper MatrixTreeTheorem<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge
        {
            if (graph.Length == 0) return new LaplacianWrapper(new ArrayMatrix<int, IntOperator>(Array.Empty<int>(), 0, 0));
            Contract.Assert(!graph[0].IsDirected, "graph は無向グラフである必要があります");
            return new LaplacianWrapper(graph.Laplacian());
        }

        public readonly struct LaplacianWrapper
        {
            readonly ArrayMatrix<int, IntOperator> Matrix;
            public LaplacianWrapper(ArrayMatrix<int, IntOperator> m) { Matrix = m; }

            /// <summary>
            /// 全域木の個数を返します。
            /// </summary>
            public StaticModInt<T> Calc<T>() where T : struct, IStaticMod
            {
                var orig = Matrix.Value;
                var n = Matrix.Height - 1;
                var a = new StaticModInt<T>[n][];
                for (int i = 0; i < a.Length; i += n)
                {
                    a[i] = new StaticModInt<T>[n];
                    ref var head = ref a[i][0];
                    var src = orig.AsSpan(i * (n + 1), n);
                    for (int j = 0; j < src.Length; j++)
                    {
                        Unsafe.Add(ref head, j) = src[j];
                    }
                }
                return ArrayMatrix<StaticModInt<T>, StaticModIntOperator<T>>.DeterminantImpl(a);
            }
        }
    }
}
