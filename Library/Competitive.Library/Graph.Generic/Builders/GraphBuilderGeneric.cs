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

        public SimpleGraph<GraphEdge<T>> ToGraph()
            => GraphBuilderLogic.ToGraph<SimpleGraph<GraphEdge<T>>, GraphEdge<T>>(edgeContainer);
        public TreeGraph<TreeNode<GraphEdge<T>>, GraphEdge<T>> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<TreeGraph<TreeNode<GraphEdge<T>>, GraphEdge<T>>, TreeNode<GraphEdge<T>>, GraphEdge<T>>(edgeContainer, root);
    }

    public readonly struct GraphEdge<T> : IGraphData<T>, IGraphEdge<GraphEdge<T>>, IEquatable<GraphEdge<T>>
    {
        public static GraphEdge<T> None { get; } = new GraphEdge<T>(-1, default);
        public GraphEdge(int to, T data)
        {
            To = to;
            Data = data;
        }
        public T Data { get; }
        public int To { get; }
        [凾(256)] public static implicit operator int(GraphEdge<T> e) => e.To;
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
}
