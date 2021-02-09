using AtCoder;
using AtCoder.Internal;
using System.Diagnostics;

namespace Kzrnm.Competitive
{
    public interface IWGraph<T, TOp, TNode, TEdge> : IGraph<TNode, TEdge>
        where TOp : struct, IAdditionOperator<T>
        where TNode : IWGraphNode<T, TEdge, TOp>
        where TEdge : IWEdge<T>
    { }
    public interface IWTreeGraph<T, TOp, TNode, TEdge> : ITreeGraph<TNode, TEdge>
        where TOp : struct, IAdditionOperator<T>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWEdge<T>
    { }
    public class WGraph<T, TOp, TNode, TEdge> : IWGraph<T, TOp, TNode, TEdge>
        where TOp : struct, IAdditionOperator<T>
        where TNode : IWGraphNode<T, TEdge, TOp>
        where TEdge : IWEdge<T>
    {
        public CSR<TEdge> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public WGraph(TNode[] array, CSR<TEdge> edges)
        {
            this.Nodes = array;
            this.Edges = edges;
        }
    }
    public class WTreeGraph<T, TOp, TNode, TEdge> : IWTreeGraph<T, TOp, TNode, TEdge>
        where TOp : struct, IAdditionOperator<T>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWEdge<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public int Root { get; }
        public WTreeGraph(TNode[] array,int root)
        {
            this.Root = root;
            this.Nodes = array;
        }
    }
}
