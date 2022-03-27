using AtCoder.Internal;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class SimpleGraph<TNode, TEdge> : IGraph<TNode, TEdge>
        where TNode : IGraphNode<TEdge>
        where TEdge : IGraphEdge
    {
        public CSR<TEdge> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        [凾(256)]
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public SimpleGraph(TNode[] array, CSR<TEdge> edges)
        {
            Nodes = array;
            Edges = edges;
        }
    }
    public class TreeGraph<TNode, TEdge> : ITreeGraph<TNode, TEdge>
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
        public TreeGraph(TNode[] array, int root)
        {
            Root = root;
            Nodes = array;
        }
    }
}
