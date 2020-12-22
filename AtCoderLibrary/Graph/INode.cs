#pragma warning disable CA1819 // Properties should not return arrays
namespace AtCoder.Graph
{
    public interface IReversable<T>
    {
        T Reversed(int from);
    }
    public interface IEdge
    {
        int To { get; }
    }
    public interface INode<out TEdge> where TEdge : IEdge
    {
        int Index { get; }
        TEdge[] Roots { get; }
        TEdge[] Children { get; }
        bool IsDirected { get; }
    }
    public interface ITreeNode<out TEdge> where TEdge : IEdge
    {
        int Index { get; }
        TEdge Root { get; }
        TEdge[] Children { get; }
        int Depth { get; }
    }
}
