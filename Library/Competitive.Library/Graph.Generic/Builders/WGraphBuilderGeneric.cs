using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 辺にデータを持つ重み付きデータ付きのグラフを構築する
    /// </summary>
    /// <typeparam name="T">辺の重みの型</typeparam>
    /// <typeparam name="S">辺のデータ型</typeparam>
    public class WGraphBuilder<T, S> : Internal.Graph.Builder<WGraph<T, GraphNode<WEdge<T, S>>, WEdge<T, S>>, WTreeGraph<T, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>, GraphNode<WEdge<T, S>>, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>
        where T : IAdditionOperators<T, T, T>
    {
        public WGraphBuilder(int size, bool isDirected) : base(size, isDirected) { }
        [凾(256)]
        public void Add(int from, int to, T value, S data) => edges.Add(from, new(to, value, data));
    }

    /// <summary>
    /// 重み付きデータ付きの辺
    /// </summary>
    /// <typeparam name="T">辺の重みの型</typeparam>
    /// <typeparam name="S">辺のデータ型</typeparam>
    /// <param name="To">行き先</param>
    /// <param name="Value">辺の重み</param>
    /// <param name="Data">データ</param>
    [DebuggerDisplay(nameof(To) + " = {" + nameof(To) + "}, " + nameof(Value) + " = {" + nameof(Value) + "}, " + nameof(Data) + " = {" + nameof(Data) + "}")]
    public readonly record struct WEdge<T, S>(int To, T Value, S Data) : IWGraphEdge<T>, IGraphData<S>, IGraphEdge<WEdge<T, S>>
    {
        public static WEdge<T, S> None => new(-1, default, default);
        [凾(256)] public static implicit operator int(WEdge<T, S> e) => e.To;
        [凾(256)] public WEdge<T, S> Reversed(int from) => new(from, Value, Data);
    }
}
