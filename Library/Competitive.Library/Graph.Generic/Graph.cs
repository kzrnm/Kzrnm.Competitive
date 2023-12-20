using AtCoder.Internal;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class SimpleGraph<TEdge> : IGraph<SimpleGraph<TEdge>, TEdge>
        where TEdge : IGraphEdge<TEdge>
    {
        public Csr<TEdge> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal GraphNode<TEdge>[] Nodes { get; }
        [凾(256)]
        public GraphNode<TEdge>[] AsArray() => Nodes;
        [凾(256)]
        static SimpleGraph<TEdge> IGraph<SimpleGraph<TEdge>, TEdge>.Graph(GraphNode<TEdge>[] nodes, Csr<TEdge> edges)
            => new(nodes, edges);

        public GraphNode<TEdge> this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public SimpleGraph(GraphNode<TEdge>[] array, Csr<TEdge> edges)
        {
            Nodes = array;
            Edges = edges;
        }
    }
    public class TreeGraph<TNode, TEdge> : ITreeGraph<TreeGraph<TNode, TEdge>, TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        [凾(256)]
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public int Root { get; }
        public HeavyLightDecomposition<TNode, TEdge> HlDecomposition { get; }
        public TreeGraph(TNode[] array, int root, HeavyLightDecomposition<TNode, TEdge> hl)
        {
            Root = root;
            Nodes = array;
            HlDecomposition = hl;
        }

        [凾(256)]
        static TreeGraph<TNode, TEdge> ITreeGraph<TreeGraph<TNode, TEdge>, TNode, TEdge>.Tree(TNode[] nodes, int root, HeavyLightDecomposition<TNode, TEdge> hl)
            => new(nodes, root, hl);
    }
}
