namespace AtCoder
{
    public interface IWGraph<T, TOp, out TNode, out TEdge> : IGraph<TNode, TEdge>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
        where TNode : IWNode<T, TEdge, TOp>
        where TEdge : IWEdge<T>
    { }
    public interface IWTreeGraph<T, TOp, out TNode, out TEdge> : ITreeGraph<TNode, TEdge>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWEdge<T>
    { }
    public class WGraph<T, TOp, TNode, TEdge> : IWGraph<T, TOp, TNode, TEdge>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
        where TNode : IWNode<T, TEdge, TOp>
        where TEdge : IWEdge<T>
    {
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public WGraph(TNode[] array)
        {
            this.Nodes = array;
        }
    }
    public class WTreeGraph<T, TOp, TNode, TEdge> : IWTreeGraph<T, TOp, TNode, TEdge>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
        where TNode : ITreeNode<TEdge>
        where TEdge : IWEdge<T>
    {
        internal TNode[] Nodes { get; }
        public TNode[] AsArray() => Nodes;
        public TNode this[int index] => Nodes[index];
        public int Length => Nodes.Length;
        public WTreeGraph(TNode[] array)
        {
            this.Nodes = array;
        }
    }
}
