namespace Kzrnm.Competitive
{
    public static class ___CastToUnweighted
    {
        public static IGraph<TNode, TEdge> AsUnweighted<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> g)
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
            => g;

        public static ITreeGraph<TNode, TEdge> AsUnweighted<T, TNode, TEdge>(this IWTreeGraph<T, TNode, TEdge> g)
            where TNode : ITreeNode<TEdge>
            where TEdge : IWGraphEdge<T>
            => g;
    }
}
