using AtCoder;
namespace Kzrnm.Competitive
{
    public interface IWEdge<T> : IEdge
    {
        T Value { get; }
    }
    public interface IWGraphNode<T, out TEdge, TOp> : IGraphNode<TEdge>
        where TEdge : IWEdge<T>
        where TOp : struct, IAdditionOperator<T>
    {
    }
}
