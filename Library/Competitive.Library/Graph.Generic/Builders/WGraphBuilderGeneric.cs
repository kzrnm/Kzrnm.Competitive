using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraphBuilder<T, S>
        where T : IAdditionOperators<T, T, T>
    {
        readonly EdgeContainer<WEdge<T, S>> edgeContainer;
        public WGraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to, T value, S data) => edgeContainer.Add(from, new(to, value, data));

        public WGraph<T, WEdge<T, S>> ToGraph()
            => edgeContainer.ToGraph<WGraph<T, WEdge<T, S>>>();

        public WTreeGraph<T, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>> ToTree(int root = 0)
            => edgeContainer.ToTree<WTreeGraph<T, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>, WTreeNode<T, WEdge<T, S>>>(root);
    }

    [DebuggerDisplay(nameof(To) + " = {" + nameof(To) + "}, " + nameof(Value) + " = {" + nameof(Value) + "}, " + nameof(Data) + " = {" + nameof(Data) + "}")]
    public readonly record struct WEdge<T, S>(int To, T Value, S Data) : IWGraphEdge<T>, IGraphData<S>, IGraphEdge<WEdge<T, S>>
    {
        public static WEdge<T, S> None => new(-1, default, default);
        [凾(256)] public static implicit operator int(WEdge<T, S> e) => e.To;
        [凾(256)] public WEdge<T, S> Reversed(int from) => new(from, Value, Data);
    }
}
