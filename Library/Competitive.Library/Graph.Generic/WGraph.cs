using AtCoder.Internal;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public interface IWGraph<T, Te> : IGraph<Te>
        where Te : IWGraphEdge<T>
    { }
    public interface IWGraph<T, Tn, Te> : IWGraph<T, Te>, IGraph<Tn, Te>
        where Tn : GraphNode<Te>
        where Te : IWGraphEdge<T>
    { }
    public interface IWTreeGraph<T, Tn, Te> : ITreeGraph<Tn, Te>
        where Tn : ITreeNode<Te>
        where Te : IWGraphEdge<T>
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

    public class WGraph<T, Tn, Te> : IWGraph<T, Tn, Te>, IGraph<WGraph<T, Tn, Te>, Tn, Te>
        where Te : IWGraphEdge<T>
        where Tn : GraphNode<Te>
    {
        public Csr<Te> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal Tn[] Nodes { get; }
        [凾(256)]
        public Tn[] AsArray() => Nodes;
        public Tn this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public WGraph(Tn[] array, Csr<Te> edges)
        {
            Nodes = array;
            Edges = edges;
        }
        [凾(256)]
        static WGraph<T, Tn, Te> IGraph<WGraph<T, Tn, Te>, Tn, Te>.Graph(Tn[] nodes, Csr<Te> edges)
            => new(nodes, edges);
    }
    public class WTreeGraph<T, TNode, TEdge> : IWTreeGraph<T, TNode, TEdge>, ITreeGraph<WTreeGraph<T, TNode, TEdge>, TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWGraphEdge<T>, IGraphEdge<TEdge>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        [凾(256)]
        public TNode[] AsArray() => Nodes;

        [凾(256)]
        static WTreeGraph<T, TNode, TEdge> ITreeGraph<WTreeGraph<T, TNode, TEdge>, TNode, TEdge>.Tree(TNode[] nodes, int root, HeavyLightDecomposition<TNode, TEdge> hl)
            => new(nodes, root, hl);

        public TNode this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public int Root { get; }
        public HeavyLightDecomposition<TNode, TEdge> HlDecomposition { get; }

        public WTreeGraph(TNode[] array, int root, HeavyLightDecomposition<TNode, TEdge> hl)
        {
            Root = root;
            Nodes = array;
            HlDecomposition = hl;
        }
    }

    public class WTreeNode<T, Te> : TreeNode<Te>, ITreeNode<WTreeNode<T, Te>, Te>, IWTreeNode<T>
        where T : IAdditionOperators<T, T, T>
        where Te : IWGraphEdge<T>, IGraphEdge<Te>
    {
        public WTreeNode(int i, int size, Te parent, int depth, T depthLength, Te[] children)
            : base(i, size, parent, depth, children)
        {
            DepthLength = depthLength;
        }
        public T DepthLength { get; }

        [凾(256)]
        static WTreeNode<T, Te> ITreeNode<WTreeNode<T, Te>, Te>.Node(int i, int size, WTreeNode<T, Te> parent, Te parentEdge, Te[] children)
            => new(i, size, parentEdge.Reversed(parent.Index), parent.Depth + 1, parent.DepthLength + parentEdge.Value, children);

        [凾(256)]
        static WTreeNode<T, Te> ITreeNode<WTreeNode<T, Te>, Te>.RootNode(int i, int size, Te[] children)
            => new(i, size, Te.None, 0, default, children);
    }
}
