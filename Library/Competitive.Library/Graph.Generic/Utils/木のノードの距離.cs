using System;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class 木のノードの距離
    {
        /// <summary>
        /// <paramref name="u"/>と<paramref name="v"/>の距離(間にあるノード数)を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public static int Distance<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree, int u, int v)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
        {
            var l = tree.HlDecomposition.LowestCommonAncestor(u, v);
            return tree[u].Depth + tree[v].Depth - tree[l].Depth * 2;
        }
        /// <summary>
        /// <paramref name="u"/>と<paramref name="v"/>の距離を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public static T DistanceLength<T, TNode, TEdge>(this IWTreeGraph<T, TNode, TEdge> tree, int u, int v)
            where T : struct, IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
            where TNode : ITreeNode<TEdge>, IWTreeNode<T>
            where TEdge : IWGraphEdge<T>
        {
            var l = tree.HlDecomposition.LowestCommonAncestor(u, v);
            var ld = tree[l].DepthLength;
            return tree[u].DepthLength + tree[v].DepthLength - ld - ld;
        }

        /// <summary>
        /// 木の最遠頂点対(木の直径をなす頂点の組)を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static (int, int) Diameter<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
        {
            var t = tree.AsArray();
            var u = tree.Root;
            int v = tree.Root;
            var max = 0;

            for (int i = 0; i < t.Length; i++)
                if (max.UpdateMax(tree[i].Depth))
                    u = i;

            for (int i = 0; i < t.Length; i++)
                if (max.UpdateMax(tree.Distance(u, i)))
                    v = i;
            return (u, v);
        }

        /// <summary>
        /// 木の最遠頂点対(木の直径をなす頂点の組)を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static (int, int) DiameterLength<T, TNode, TEdge>(this IWTreeGraph<T, TNode, TEdge> tree)
            where T : struct, IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IComparable<T>
            where TNode : ITreeNode<TEdge>, IWTreeNode<T>
            where TEdge : IWGraphEdge<T>
        {
            var t = tree.AsArray();
            var u = tree.Root;
            int v = tree.Root;
            T max = default;

            for (int i = 0; i < t.Length; i++)
                if (max.UpdateMax(tree[i].DepthLength))
                    u = i;

            for (int i = 0; i < t.Length; i++)
                if (max.UpdateMax(tree.DistanceLength(u, i)))
                    v = i;
            return (u, v);
        }
    }
}
