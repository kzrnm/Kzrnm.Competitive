using Kzrnm.Competitive.IO;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using TreeNode = TreeNode<GraphEdge>;
    public class GraphBuilder
    {
        internal readonly EdgeContainer<GraphEdge> edgeContainer;

        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<GraphEdge>(size, isDirected);
        }
        public static GraphBuilder Create(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new GraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0());
            return gb;
        }
        public static GraphBuilder CreateTree(int count, ConsoleReader cr)
        {
            var gb = new GraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0(), cr.Int0());
            return gb;
        }
        public static GraphBuilder<int> CreateWithEdgeIndex(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new GraphBuilder<int>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0(), i);
            return gb;
        }
        [凾(256)]
        public void Add(int from, int to) => edgeContainer.Add(from, new GraphEdge(to));

        public SimpleGraph<GraphEdge> ToGraph()
            => GraphBuilderLogic.ToGraph<SimpleGraph<GraphEdge>, GraphEdge>(edgeContainer);

        public TreeGraph<TreeNode, GraphEdge> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<TreeGraph<TreeNode, GraphEdge>, TreeNode, GraphEdge>(edgeContainer, root);
    }

    public readonly struct GraphEdge : IGraphEdge<GraphEdge>, IEquatable<GraphEdge>
    {
        public static GraphEdge None { get; } = new GraphEdge(-1);
        public GraphEdge(int to)
        {
            To = to;
        }
        public int To { get; }
        [凾(256)] public static implicit operator int(GraphEdge e) => e.To;
        public override string ToString() => To.ToString();
        [凾(256)] public GraphEdge Reversed(int from) => new GraphEdge(from);

        public override int GetHashCode() => To;
        public override bool Equals(object obj) => obj is GraphEdge edge && Equals(edge);
        public bool Equals(GraphEdge other) => To == other.To;
        public static bool operator ==(GraphEdge left, GraphEdge right) => left.Equals(right);
        public static bool operator !=(GraphEdge left, GraphEdge right) => !left.Equals(right);
    }
}