using AtCoder.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class GraphBuilder<T>
    {
        internal readonly EdgeContainer<GraphEdge<T>> edgeContainer;
        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<GraphEdge<T>>(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to, T data) => edgeContainer.Add(from, new GraphEdge<T>(to, data));

        public SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>> ToGraph()
            => GraphBuilderLogic.ToGraph<SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>>, GraphNode<GraphEdge<T>>, GraphEdge<T>, TOp>(edgeContainer);
        public TreeGraph<TreeNode<T>, GraphEdge<T>> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<TreeGraph<TreeNode<T>, GraphEdge<T>>, TreeNode<T>, GraphEdge<T>, TOp>(edgeContainer, root);
        struct TOp :
            IGraphBuildOperator<SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>>, GraphNode<GraphEdge<T>>, GraphEdge<T>>,
            ITreeBuildOperator<TreeGraph<TreeNode<T>, GraphEdge<T>>, TreeNode<T>, GraphEdge<T>>
        {
            [凾(256)] public SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>> Graph(GraphNode<GraphEdge<T>>[] nodes, CSR<GraphEdge<T>> edges) => new SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>>(nodes, edges);
            [凾(256)] public GraphNode<GraphEdge<T>> Node(int i, GraphEdge<T>[] roots, GraphEdge<T>[] children) => new GraphNode<GraphEdge<T>>(i, roots, children);

            [凾(256)] public TreeGraph<TreeNode<T>, GraphEdge<T>> Tree(TreeNode<T>[] nodes, int root) => new TreeGraph<TreeNode<T>, GraphEdge<T>>(nodes, root);
            [凾(256)]
            public TreeNode<T> TreeNode(int i, int size, TreeNode<T> parent, GraphEdge<T> edge, GraphEdge<T>[] children)
                => new TreeNode<T>(i, size, edge.Reversed(parent.Index), parent.Depth + 1, children);
            [凾(256)] public TreeNode<T> TreeRootNode(int i, int size, GraphEdge<T>[] children) => new TreeNode<T>(i, size, GraphEdge<T>.None, 0, children);
        }
    }

    public readonly struct GraphEdge<T> : IGraphEdge, IGraphData<T>, IReversable<GraphEdge<T>>, IEquatable<GraphEdge<T>>
    {
        public static GraphEdge<T> None { get; } = new GraphEdge<T>(-1, default);
        public GraphEdge(int to, T data)
        {
            To = to;
            Data = data;
        }
        public T Data { get; }
        public int To { get; }
        public override string ToString() => $"to:{To}, Data:{Data}";
        [凾(256)]
        public void Deconstruct(out int to, out T data)
        {
            to = To;
            data = Data;
        }
        [凾(256)]
        public GraphEdge<T> Reversed(int from) => new GraphEdge<T>(from, Data);

        public override int GetHashCode() => To;
        public override bool Equals(object obj) => obj is GraphEdge<T> edge && Equals(edge);
        public bool Equals(GraphEdge<T> other) => To == other.To;
        public static bool operator ==(GraphEdge<T> left, GraphEdge<T> right) => left.Equals(right);
        public static bool operator !=(GraphEdge<T> left, GraphEdge<T> right) => !left.Equals(right);
    }

    public class GraphNode<TEdge> : IGraphNode<TEdge>, IEquatable<GraphNode<TEdge>>
        where TEdge : IGraphEdge
    {
        public GraphNode(int i, TEdge[] roots, TEdge[] children)
        {
            Index = i;
            Roots = roots;
            Children = children;
        }
        public int Index { get; }
        public TEdge[] Roots { get; }
        public TEdge[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is GraphNode<TEdge> d && Equals(d);
        public bool Equals(GraphNode<TEdge> other) => Index == other?.Index;
        public override int GetHashCode() => Index;
    }
    public class TreeNode<T> : ITreeNode<GraphEdge<T>>, IEquatable<TreeNode<T>>
    {
        public TreeNode(int i, int size, GraphEdge<T> root, int depth, GraphEdge<T>[] children)
        {
            Index = i;
            Root = root;
            Children = children;
            Depth = depth;
            Size = size;
        }
        public int Index { get; }
        public GraphEdge<T> Root { get; }
        public GraphEdge<T>[] Children { get; }
        public int Depth { get; }
        public int Size { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is TreeNode<T> node && Equals(node);
        public bool Equals(TreeNode<T> other) => other != null && Index == other.Index;
        public override int GetHashCode() => Index;
    }

}
