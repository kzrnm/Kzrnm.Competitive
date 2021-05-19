#pragma warning disable CA1819 // Properties should not return arrays
using System;

namespace Kzrnm.Competitive
{
    public interface IReversable<T> where T : IGraphEdge
    {
        T Reversed(int from);
    }
    public interface IGraphEdge
    {
        int To { get; }
    }
    public interface IWGraphEdge<T> : IGraphEdge
    {
        T Value { get; }
    }
    public interface IGraphNode<out TEdge> where TEdge : IGraphEdge
    {
        int Index { get; }
        TEdge[] Roots { get; }
        TEdge[] Children { get; }
        bool IsDirected { get; }
    }
    public interface ITreeNode<TEdge> where TEdge : IGraphEdge
    {
        int Index { get; }
        TEdge Root { get; }
        TEdge[] Children { get; }
        int Depth { get; }
    }
}
