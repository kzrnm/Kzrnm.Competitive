using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Gde = GraphEdge;
    using Tn = TreeNode<GraphEdge>;
    /// <summary>
    /// グラフを構築する
    /// </summary>
    public class GraphBuilder : Internal.Graph.Builder<SimpleGraph<GraphNode<Gde>, Gde>, TreeGraph<Tn, Gde>, GraphNode<Gde>, Tn, Gde>
    {
        public GraphBuilder(int size, bool isDirected) : base(size, isDirected) { }
        [凾(256)]
        public void Add(int from, int to) => edges.Add(from, new(to));
    }

    [DebuggerDisplay("{" + nameof(To) + "}")]
    public readonly record struct GraphEdge(int To) : IGraphEdge<Gde>
    {
        public static Gde None => new(-1);
        [凾(256)] public static implicit operator int(Gde e) => e.To;
        [凾(256)] public Gde Reversed(int from) => new(from);
    }
}