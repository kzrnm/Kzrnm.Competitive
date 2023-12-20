namespace Kzrnm.Competitive
{
    public interface IWGraph<T, TEdge> : IGraph<TEdge>
        where TEdge : IWGraphEdge<T>
    { }
    public interface IWTreeGraph<T, TNode, TEdge> : ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWGraphEdge<T>
    { }

    public interface IWGraphEdge<T> : IGraphEdge
    {
        /// <summary>
        /// 重み
        /// </summary>
        T Value { get; }
    }
    public interface IWTreeNode<T>
    {
        /// <summary>
        /// 根からの重みの総和
        /// </summary>
        public T DepthLength { get; }
    }
}
