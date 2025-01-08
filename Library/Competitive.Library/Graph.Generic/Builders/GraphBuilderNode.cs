using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Gde = GraphEdge;
    using Tn = TreeNode<GraphEdge>;
    /// <summary>
    /// ノードにデータを持つグラフを構築する
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    public class GraphBuilderNode<T> : Internal.Graph.BuilderBase<SimpleGraph<GraphNodeData<T>, Gde>, TreeGraph<TreeNodeData<T>, Gde>, GraphNodeData<T>, TreeNodeData<T>, Gde>
    {
        private readonly T[] vals;
        public GraphBuilderNode(T[] values, bool isDirected) : base(values.Length, isDirected)
        {
            vals = values;
        }
        [凾(256)]
        public void Add(int from, int to) => edges.Add(from, new(to));

        protected override GraphNodeData<T> Node(int i, Gde[] parents, Gde[] children)
            => new(i, parents, children, vals[i]);

        protected override TreeNodeData<T> RootNode(int i, int size, Gde[] children)
            => new(i, size, Gde.None, 0, children, vals[i]);

        protected override TreeNodeData<T> TreeNode(int i, int size, TreeNodeData<T> parent, Gde edge, Gde[] children)
            => new(i, size, edge.Reversed(parent.Index), parent.Depth + 1, children, vals[i]);
    }

    /// <summary>
    /// データ付きのノード
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    public class GraphNodeData<T> : GraphNode<Gde>, IGraphNode<Gde>
    {
        public GraphNodeData(int i, Gde[] parents, Gde[] children, T value)
            : base(i, parents, children)
        {
            Value = value;
        }
        public T Value { get; }

        public override string ToString() => $"Value: {Value}, children: {string.Join(",", Children)}";
        public override int GetHashCode() => Index;
    }

    /// <summary>
    /// データ付きのノード(木)
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    public class TreeNodeData<T> : Tn, ITreeNode<Gde>
    {
        public TreeNodeData(int i, int size, Gde parent, int depth, Gde[] children, T value)
            : base(i, size, parent, depth, children)
        {
            Value = value;
        }
        public T Value { get; }

        public override string ToString() => $"Value: {Value}, children: {string.Join(",", Children)}";
        public override int GetHashCode() => Index;
    }
}