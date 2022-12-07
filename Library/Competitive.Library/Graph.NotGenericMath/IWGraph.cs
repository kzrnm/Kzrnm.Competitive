using AtCoder.Operators;

namespace Kzrnm.Competitive
{
    public interface IWGraph<T, TOp, TNode, TEdge> : IGraph<TNode, TEdge>
        where TOp : struct, IAdditionOperator<T>
        where TNode : IGraphNode<TEdge>
        where TEdge : IWGraphEdge<T>
    { }
    public interface IWTreeGraph<T, TOp, TNode, TEdge> : ITreeGraph<TNode, TEdge>
        where TOp : struct, IAdditionOperator<T>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWGraphEdge<T>
    { }
}
