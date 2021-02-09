using AtCoder.Internal;
using System.Diagnostics;

namespace Kzrnm.Competitive
{
    public interface IGraph<TNode, TEdge>
        where TNode : IGraphNode<TEdge>
        where TEdge : IEdge
    {
        CSR<TEdge> Edges { get; }
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
    }
    public interface ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IEdge
    {
        int Root { get; }
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
    }
    public class SimpleGraph<TNode, TEdge> : IGraph<TNode, TEdge>
        where TNode : IGraphNode<TEdge>
        where TEdge : IEdge
    {
        public CSR<TEdge> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public SimpleGraph(TNode[] array, CSR<TEdge> edges)
        {
            this.Nodes = array;
            this.Edges = edges;
        }
    }
    public class TreeGraph<TNode, TEdge> : ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IEdge
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public int Root { get; }
        public TreeGraph(TNode[] array, int root)
        {
            this.Root = root;
            this.Nodes = array;
        }
    }
}
