using System;

namespace Kzrnm.Competitive
{
    public class WGraphNode<T, TEdge> : IGraphNode<TEdge>, IEquatable<WGraphNode<T, TEdge>>
        where TEdge : IWGraphEdge<T>
    {
        public WGraphNode(int i, TEdge[] roots, TEdge[] children)
        {
            this.Index = i;
            this.Roots = roots;
            this.Children = children;
        }
        public int Index { get; }
        public TEdge[] Roots { get; }
        public TEdge[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is WGraphNode<T, TEdge> d && this.Equals(d);
        public bool Equals(WGraphNode<T, TEdge> other) => this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
    public class WTreeNode<T, TEdge> : ITreeNode<TEdge>, IEquatable<WTreeNode<T, TEdge>>
        where TEdge : IWGraphEdge<T>
    {
        public WTreeNode(int i, TEdge root, int depth, T depthLength, TEdge[] children)
        {
            this.Index = i;
            this.Root = root;
            this.Children = children;
            this.Depth = depth;
            this.DepthLength = depthLength;
        }
        public int Index { get; }
        public TEdge Root { get; }
        public TEdge[] Children { get; }
        public int Depth { get; }
        public T DepthLength { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is WTreeNode<T, TEdge> node && this.Equals(node);
        public bool Equals(WTreeNode<T, TEdge> other) => other != null && this.Index == other.Index;
        public override int GetHashCode() => this.Index;
    }
}
