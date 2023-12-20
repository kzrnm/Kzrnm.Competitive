using AtCoder;
using AtCoder.Internal;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraph<T, TEdge> : IWGraph<T, TEdge>, IGraph<WGraph<T, TEdge>, TEdge>
        where TEdge : IWGraphEdge<T>
    {
        public Csr<TEdge> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal GraphNode<TEdge>[] Nodes { get; }
        [凾(256)]
        public GraphNode<TEdge>[] AsArray() => Nodes;
        public GraphNode<TEdge> this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public WGraph(GraphNode<TEdge>[] array, Csr<TEdge> edges)
        {
            Nodes = array;
            Edges = edges;
        }
        [凾(256)]
        static WGraph<T, TEdge> IGraph<WGraph<T, TEdge>, TEdge>.Graph(GraphNode<TEdge>[] nodes, Csr<TEdge> edges)
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
}
