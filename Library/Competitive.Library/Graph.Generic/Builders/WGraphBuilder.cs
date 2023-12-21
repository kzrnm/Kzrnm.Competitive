using Kzrnm.Competitive.IO;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraphBuilder<T> where T : IAdditionOperators<T, T, T>
    {
        internal readonly EdgeContainer<WEdge<T>> edgeContainer;
        public WGraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to, T value) => edgeContainer.Add(from, new(to, value));

        public WGraph<T, WEdge<T>> ToGraph()
            => edgeContainer.ToGraph<WGraph<T, WEdge<T>>>();

        public WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>> ToTree(int root = 0)
            => edgeContainer.ToTree<WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>>, WTreeNode<T, WEdge<T>>>(root);
    }

    [DebuggerDisplay(nameof(To) + " = {" + nameof(To) + "}, " + nameof(Value) + " = {" + nameof(Value) + "}")]
    public readonly record struct WEdge<T>(int To, T Value) : IWGraphEdge<T>, IGraphEdge<WEdge<T>>
    {
        public static WEdge<T> None => new(-1, default);
        [凾(256)] public static implicit operator int(WEdge<T> e) => e.To;
        [凾(256)] public WEdge<T> Reversed(int from) => new(from, Value);
    }
}
