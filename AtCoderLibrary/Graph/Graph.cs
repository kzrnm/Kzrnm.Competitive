using AtCoder.Internal;

namespace AtCoder
{
    public interface IGraph<TNode, TEdge>
        where TNode : INode<TEdge>
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
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
    }
    public class Graph<TNode, TEdge> : IGraph<TNode, TEdge>
        where TNode : INode<TEdge>
        where TEdge : IEdge
    {
        public CSR<TEdge> Edges { get; }
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public Graph(TNode[] array, CSR<TEdge> edges)
        {
            this.Nodes = array;
            this.Edges = edges;
        }
    }
    public class TreeGraph<TNode, TEdge> : ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IEdge
    {
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public TreeGraph(TNode[] array)
        {
            this.Nodes = array;
        }
    }
}
