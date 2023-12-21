using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using TreeNode = TreeNode<GraphEdge>;
    public class GraphBuilder
    {
        readonly EdgeContainer<GraphEdge> edgeContainer;

        public GraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to) => edgeContainer.Add(from, new(to));

        public SimpleGraph<GraphEdge> ToGraph()
            => edgeContainer.ToGraph<SimpleGraph<GraphEdge>>();

        public TreeGraph<TreeNode, GraphEdge> ToTree(int root = 0)
            => edgeContainer.ToTree<TreeGraph<TreeNode, GraphEdge>, TreeNode>(root);
    }

    [DebuggerDisplay("{" + nameof(To) + "}")]
    public readonly record struct GraphEdge(int To) : IGraphEdge<GraphEdge>
    {
        public static GraphEdge None => new(-1);
        [凾(256)] public static implicit operator int(GraphEdge e) => e.To;
        [凾(256)] public GraphEdge Reversed(int from) => new(from);
    }
}