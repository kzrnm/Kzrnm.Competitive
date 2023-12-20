namespace Kzrnm.Competitive
{
    public static class ___CastToUnweighted
    {
        public static IGraph<TEdge> AsUnweighted<T, TEdge>(this IWGraph<T, TEdge> g)
            where TEdge : IWGraphEdge<T>
            => g;

        public static ITreeGraph<TNode, TEdge> AsUnweighted<T, TNode, TEdge>(this IWTreeGraph<T, TNode, TEdge> g)
            where TNode : ITreeNode<TEdge>
            where TEdge : IWGraphEdge<T>
            => g;
    }
}
