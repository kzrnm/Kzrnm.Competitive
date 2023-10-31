using AtCoder.Internal;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraph<T, TNode, TEdge> : IWGraph<T, TNode, TEdge>
        where TNode : IGraphNode<TEdge>
        where TEdge : IWGraphEdge<T>
    {
        public Csr<TEdge> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        [凾(256)]
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public WGraph(TNode[] array, Csr<TEdge> edges)
        {
            Nodes = array;
            Edges = edges;
        }
    }
    public class WTreeGraph<T, TNode, TEdge> : IWTreeGraph<T, TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWGraphEdge<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        [凾(256)]
        public TNode[] AsArray() => Nodes;
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
