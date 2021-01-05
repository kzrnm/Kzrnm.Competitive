namespace AtCoder
{
    public interface IWEdge<T> : IEdge
    {
        T Value { get; }
    }
    public interface IWNode<T, out TEdge, TOp> : INode<TEdge>
        where TEdge : IWEdge<T>
        where TOp : struct, IAdditionOperator<T>
    {
    }
}
