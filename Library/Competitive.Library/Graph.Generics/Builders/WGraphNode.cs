using System;

namespace Kzrnm.Competitive
{
    public class WGraphNode<T, TEdge> : IGraphNode<TEdge>, IEquatable<WGraphNode<T, TEdge>>
        where TEdge : IWGraphEdge<T>
    {
        public WGraphNode(int i, TEdge[] roots, TEdge[] children)
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
        public override bool Equals(object obj) => obj is WGraphNode<T, TEdge> d && Equals(d);
        public bool Equals(WGraphNode<T, TEdge> other) => Index == other.Index;
        public override int GetHashCode() => Index;
    }
    public class WTreeNode<T, TEdge> : ITreeNode<TEdge>, IEquatable<WTreeNode<T, TEdge>>
        where TEdge : IWGraphEdge<T>
    {
        public WTreeNode(int i, int size, TEdge root, int depth, T depthLength, TEdge[] children)
        {
            Index = i;
            Root = root;
            Children = children;
            Depth = depth;
            DepthLength = depthLength;
            Size = size;
        }
        public int Index { get; }
        public TEdge Root { get; }
        public TEdge[] Children { get; }
        public int Depth { get; }
        public int Size { get; }
        /// <summary>
        /// 根からの重みの総和
        /// </summary>
        public T DepthLength { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is WTreeNode<T, TEdge> node && Equals(node);
        public bool Equals(WTreeNode<T, TEdge> other) => other != null && Index == other.Index;
        public override int GetHashCode() => Index;
    }
}
