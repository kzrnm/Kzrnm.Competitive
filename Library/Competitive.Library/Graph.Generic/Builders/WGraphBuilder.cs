using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 重み付きのグラフを構築する
    /// </summary>
    /// <typeparam name="T">辺の重みの型</typeparam>
    public class WGraphBuilder<T> : Internal.Graph.Builder<WGraph<T, GraphNode<WEdge<T>>, WEdge<T>>, WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>>, GraphNode<WEdge<T>>, WTreeNode<T, WEdge<T>>, WEdge<T>>
        where T : IAdditionOperators<T, T, T>
    {
        public WGraphBuilder(int size, bool isDirected) : base(size, isDirected) { }
        [凾(256)]
        public void Add(int from, int to, T value) => edges.Add(from, new(to, value));
    }

    /// <summary>
    /// 重み付きの辺
    /// </summary>
    /// <typeparam name="T">辺の重みの型</typeparam>
    [DebuggerDisplay(nameof(To) + " = {" + nameof(To) + "}, " + nameof(Value) + " = {" + nameof(Value) + "}")]
    public readonly record struct WEdge<T>(int To, T Value) : IWGraphEdge<T>, IGraphEdge<WEdge<T>>
    {
        public static WEdge<T> None => new(-1, default);
        [凾(256)] public static implicit operator int(WEdge<T> e) => e.To;
        [凾(256)] public WEdge<T> Reversed(int from) => new(from, Value);
    }
}
