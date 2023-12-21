using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class GraphBuilder<T>
    {
        readonly EdgeContainer<GraphEdge<T>> edgeContainer;
        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to, T data) => edgeContainer.Add(from, new(to, data));

        public SimpleGraph<GraphEdge<T>> ToGraph()
            => edgeContainer.ToGraph<SimpleGraph<GraphEdge<T>>>();
        public TreeGraph<TreeNode<GraphEdge<T>>, GraphEdge<T>> ToTree(int root = 0)
            => edgeContainer.ToTree<TreeGraph<TreeNode<GraphEdge<T>>, GraphEdge<T>>, TreeNode<GraphEdge<T>>>(root);
    }

    [DebuggerDisplay(nameof(To) + " = {" + nameof(To) + "}, " + nameof(Data) + " = {" + nameof(Data) + "}")]
    public readonly record struct GraphEdge<T>(int To, T Data) : IGraphData<T>, IGraphEdge<GraphEdge<T>>
    {
        public static GraphEdge<T> None => new(-1, default);
        [凾(256)] public static implicit operator int(GraphEdge<T> e) => e.To;
        [凾(256)] public GraphEdge<T> Reversed(int from) => new(from, Data);
    }
}
