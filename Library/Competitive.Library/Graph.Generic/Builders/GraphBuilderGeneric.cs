using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 辺にデータを持つグラフを構築する
    /// </summary>
    /// <typeparam name="T">辺のデータ型</typeparam>
    public class GraphBuilder<T> : Internal.Graph.Builder<SimpleGraph<GraphNode<GraphEdge<T>>, GraphEdge<T>>, TreeGraph<TreeNode<GraphEdge<T>>, GraphEdge<T>>, GraphNode<GraphEdge<T>>, TreeNode<GraphEdge<T>>, GraphEdge<T>>
    {
        public GraphBuilder(int size, bool isDirected) : base(size, isDirected) { }
        [凾(256)]
        public void Add(int from, int to, T data) => edges.Add(from, new(to, data));
    }

    /// <summary>
    /// データ付きの辺
    /// </summary>
    /// <typeparam name="T">辺のデータ型</typeparam>
    /// <param name="To">行き先</param>
    /// <param name="Data">データ</param>
    [DebuggerDisplay(nameof(To) + " = {" + nameof(To) + "}, " + nameof(Data) + " = {" + nameof(Data) + "}")]
    public readonly record struct GraphEdge<T>(int To, T Data) : IGraphData<T>, IGraphEdge<GraphEdge<T>>
    {
        public static GraphEdge<T> None => new(-1, default);
        [凾(256)] public static implicit operator int(GraphEdge<T> e) => e.To;
        [凾(256)] public GraphEdge<T> Reversed(int from) => new(from, Data);
    }
}
