namespace Kzrnm.Competitive
{
    public interface IWGraph<T, TNode, TEdge> : IGraph<TNode, TEdge>
        where TNode : IGraphNode<TEdge>
        where TEdge : IWGraphEdge<T>
    { }
    public interface IWTreeGraph<T, TNode, TEdge> : ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWGraphEdge<T>
    { }
}
