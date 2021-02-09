#pragma warning disable CA1819 // Properties should not return arrays
using System;

namespace AtCoder
{
    public interface IReversable<T> where T : IEdge
    {
        T Reversed(int from);
    }
    public interface IEdge
    {
        int To { get; }
    }
    public interface IGraphNode<out TEdge> where TEdge : IEdge
    {
        int Index { get; }
        TEdge[] Roots { get; }
        TEdge[] Children { get; }
        bool IsDirected { get; }
    }
    public interface ITreeNode<TEdge> where TEdge : IEdge
    {
        int Index { get; }
        TEdge Root { get; }
        TEdge[] Children { get; }
        int Depth { get; }
    }
}
