#pragma warning disable CA1819 // Properties should not return arrays
using System;

namespace Kzrnm.Competitive
{
    public class GraphNode<TEdge> : IGraphNode<TEdge>, IEquatable<GraphNode<TEdge>>
        where TEdge : IGraphEdge
    {
        public GraphNode(int i, TEdge[] roots, TEdge[] children)
        {
            Index = i;
            Roots = roots;
            Children = children;
        }
        public int Index { get; }
        public TEdge[] Roots { get; }
        public TEdge[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is GraphNode<TEdge> d && Equals(d);
        public bool Equals(GraphNode<TEdge> other) => Index == other?.Index;
        public override int GetHashCode() => Index;
    }
    public class TreeNode<T> : ITreeNode<GraphEdge<T>>, IEquatable<TreeNode<T>>
    {
        public TreeNode(int i, GraphEdge<T> root, int depth, GraphEdge<T>[] children)
        {
            Index = i;
            Root = root;
            Children = children;
            Depth = depth;
        }
        public int Index { get; }
        public GraphEdge<T> Root { get; }
        public GraphEdge<T>[] Children { get; }
        public int Depth { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is TreeNode<T> node && Equals(node);
        public bool Equals(TreeNode<T> other) => other != null && Index == other.Index;
        public override int GetHashCode() => Index;
    }
}
