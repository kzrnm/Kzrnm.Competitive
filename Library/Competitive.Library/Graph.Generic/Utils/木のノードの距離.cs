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
            var ud = tree[u].DepthLength - tree[l].DepthLength;
            var vd = tree[v].DepthLength - tree[l].DepthLength;
            return ud + vd;
        }
    }
}
