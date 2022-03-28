using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class GraphBuilder
    {
        internal readonly EdgeContainer<GraphEdge> edgeContainer;

        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<GraphEdge>(size, isDirected);
        }
        public static GraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new GraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0);
            return gb;
        }
        public static GraphBuilder<int> CreateWithEdgeIndex(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new GraphBuilder<int>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, i);
            return gb;
        }
        public static GraphBuilder CreateTree(int count, PropertyConsoleReader cr)
        {
            var gb = new GraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0, cr.Int0);
            return gb;
        }
        [凾(256)]
        public void Add(int from, int to) => edgeContainer.Add(from, new GraphEdge(to));

        public SimpleGraph<GraphNode, GraphEdge> ToGraph()
            => GraphBuilderLogic.ToGraph<SimpleGraph<GraphNode, GraphEdge>, GraphNode, GraphEdge, TOp>(edgeContainer);

        public TreeGraph<TreeNode, GraphEdge> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<TreeGraph<TreeNode, GraphEdge>, TreeNode, GraphEdge, TOp>(edgeContainer, root);
        struct TOp :
            IGraphBuildOperator<SimpleGraph<GraphNode, GraphEdge>, GraphNode, GraphEdge>,
            ITreeBuildOperator<TreeGraph<TreeNode, GraphEdge>, TreeNode, GraphEdge>
        {
            [凾(256)] public SimpleGraph<GraphNode, GraphEdge> Graph(GraphNode[] nodes, CSR<GraphEdge> edges) => new SimpleGraph<GraphNode, GraphEdge>(nodes, edges);
            [凾(256)] public GraphNode Node(int i, GraphEdge[] roots, GraphEdge[] children) => new GraphNode(i, roots, children);

            [凾(256)] public TreeGraph<TreeNode, GraphEdge> Tree(TreeNode[] nodes, int root, HeavyLightDecomposition<TreeNode, GraphEdge> hl) => new TreeGraph<TreeNode, GraphEdge>(nodes, root, hl);
            [凾(256)]
            public TreeNode TreeNode(int i, int size, TreeNode parent, GraphEdge edge, GraphEdge[] children)
                => new TreeNode(i, size, edge.Reversed(parent.Index), parent.Depth + 1, children);
            [凾(256)] public TreeNode TreeRootNode(int i, int size, GraphEdge[] children) => new TreeNode(i, size, GraphEdge.None, 0, children);
        }
    }

    public readonly struct GraphEdge : IGraphEdge, IReversable<GraphEdge>, IEquatable<GraphEdge>
    {
        public static GraphEdge None { get; } = new GraphEdge(-1);
        public GraphEdge(int to)
        {
            To = to;
        }
        public int To { get; }
        [凾(256)]
        public static implicit operator int(GraphEdge e) => e.To;
        public override string ToString() => To.ToString();
        [凾(256)]
        public GraphEdge Reversed(int from) => new GraphEdge(from);

        public override int GetHashCode() => To;
        public override bool Equals(object obj) => obj is GraphEdge edge && Equals(edge);
        public bool Equals(GraphEdge other) => To == other.To;
        public static bool operator ==(GraphEdge left, GraphEdge right) => left.Equals(right);
        public static bool operator !=(GraphEdge left, GraphEdge right) => !left.Equals(right);
    }

    public class GraphNode : IGraphNode<GraphEdge>, IEquatable<GraphNode>
    {
        public GraphNode(int i, GraphEdge[] roots, GraphEdge[] children)
        {
            Index = i;
            Roots = roots;
            Children = children;
        }
        public int Index { get; }
        public GraphEdge[] Roots { get; }
        public GraphEdge[] Children { get; }
        public bool IsDirected => Roots != Children;

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is GraphNode d && Equals(d);
        public bool Equals(GraphNode other) => Index == other?.Index;
        public override int GetHashCode() => Index;
    }
    public class TreeNode : ITreeNode<GraphEdge>, IEquatable<TreeNode>
    {
        public TreeNode(int i, int size, GraphEdge root, int depth, GraphEdge[] children)
        {
            Index = i;
            Root = root;
            Children = children;
            Depth = depth;
            Size = size;
        }
        public int Index { get; }
        public GraphEdge Root { get; }
        public GraphEdge[] Children { get; }
        public int Depth { get; }
        public int Size { get; }

        public override string ToString() => $"children: {string.Join(",", Children)}";
        public override bool Equals(object obj) => obj is TreeNode node && Equals(node);
        public bool Equals(TreeNode other) => other != null && Index == other.Index;
        public override int GetHashCode() => Index;
    }
}