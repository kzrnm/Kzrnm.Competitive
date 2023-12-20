using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraphBuilder<T, S>
        where T : IAdditionOperators<T, T, T>
    {
        internal readonly EdgeContainer<WEdge<T, S>> edgeContainer;
        public WGraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<WEdge<T, S>>(size, isDirected);
        }
        [凾(256)]
        public void Add(int from, int to, T value, S data) => edgeContainer.Add(from, new WEdge<T, S>(to, value, data));

        public WGraph<T, WEdge<T, S>> ToGraph()
            => GraphBuilderLogic.ToGraph<WGraph<T, WEdge<T, S>>, WEdge<T, S>>(edgeContainer);

        public WTreeGraph<T, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<WTreeGraph<T, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>, WTreeNode<T, WEdge<T, S>>, WEdge<T, S>>(edgeContainer, root);
    }

    public readonly struct WEdge<T, S> : IWGraphEdge<T>, IGraphData<S>, IGraphEdge<WEdge<T, S>>, IEquatable<WEdge<T, S>>
    {
        public static WEdge<T, S> None { get; } = new WEdge<T, S>(-1, default, default);
        public WEdge(int to, T value, S data)
        {
            To = to;
            Value = value;
            Data = data;
        }
        public int To { get; }
        public T Value { get; }
        public S Data { get; }

        [凾(256)] public static implicit operator int(WEdge<T, S> e) => e.To;
        public override bool Equals(object obj) => obj is WEdge<T, S> edge && Equals(edge);
        public bool Equals(WEdge<T, S> other) => To == other.To &&
            EqualityComparer<T>.Default.Equals(Value, other.Value) &&
            EqualityComparer<S>.Default.Equals(Data, other.Data);
        public override int GetHashCode() => HashCode.Combine(To, Value);
        public static bool operator ==(WEdge<T, S> left, WEdge<T, S> right) => left.Equals(right);
        public static bool operator !=(WEdge<T, S> left, WEdge<T, S> right) => !(left == right);
        public override string ToString() => $"to:{To}, Value:{Value}, Data:{Data}";
        [凾(256)]
        public void Deconstruct(out int to, out T value, out S data)
        {
            to = To;
            value = Value;
            data = Data;
        }
        [凾(256)]
        public WEdge<T, S> Reversed(int from) => new WEdge<T, S>(from, Value, Data);
    }
}
