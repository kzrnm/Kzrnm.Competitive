using AtCoder.Operators;

namespace Kzrnm.Competitive
{
    public static class ___CastToUnweighted
    {
        public static IGraph<TNode, TEdge> AsUnweighted<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> g)
            where TOp : struct, IAdditionOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
            => g;

        public static ITreeGraph<TNode, TEdge> AsUnweighted<T, TOp, TNode, TEdge>(this IWTreeGraph<T, TOp, TNode, TEdge> g)
            where TOp : struct, IAdditionOperator<T>
            where TNode : ITreeNode<TEdge>
            where TEdge : IWGraphEdge<T>
            => g;
    }
}
