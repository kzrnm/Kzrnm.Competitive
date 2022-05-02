using AtCoder.Operators;
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
        public static T DistanceLength<T, TOp, TEdge>(this IWTreeGraph<T, TOp, WTreeNode<T, TEdge>, TEdge> tree, int u, int v)
            where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>
            where TEdge : IWGraphEdge<T>
        {
            var l = tree.HlDecomposition.LowestCommonAncestor(u, v);
            var op = new TOp();
            var r = op.Add(tree[u].DepthLength, tree[v].DepthLength);
            return op.Subtract(op.Subtract(r, tree[l].DepthLength), tree[l].DepthLength);
        }
    }
}
