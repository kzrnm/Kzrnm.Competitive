using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WGraphBuilder<T>
        where T : IAdditionOperators<T, T, T>
    {
        internal readonly EdgeContainer<WEdge<T>> edgeContainer;
        public WGraphBuilder(int size, bool isDirected)
        {
            edgeContainer = new EdgeContainer<WEdge<T>>(size, isDirected);
        }
        public static WGraphBuilder<T> Create(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WGraphBuilder<T>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.Read<T>());
            return gb;
        }
        public static WGraphBuilder<T, int> CreateWithEdgeIndex(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WGraphBuilder<T, int>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.Read<T>(), i);
            return gb;
        }
        public static WGraphBuilder<T> CreateTree(int count, ConsoleReader cr)
        {
            var gb = new WGraphBuilder<T>(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.Read<T>());
            return gb;
        }
        [凾(256)]
        public void Add(int from, int to, T value) => edgeContainer.Add(from, new WEdge<T>(to, value));

        public WGraph<T, WEdge<T>> ToGraph()
            => GraphBuilderLogic.ToGraph<WGraph<T, WEdge<T>>, WEdge<T>>(edgeContainer);

        public WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>> ToTree(int root = 0)
            => GraphBuilderLogic.ToTree<WTreeGraph<T, WTreeNode<T, WEdge<T>>, WEdge<T>>, WTreeNode<T, WEdge<T>>, WEdge<T>>(edgeContainer, root);
    }

    public readonly struct WEdge<T> : IWGraphEdge<T>, IGraphEdge<WEdge<T>>, IEquatable<WEdge<T>>
    {
        public static WEdge<T> None { get; } = new WEdge<T>(-1, default);
        public WEdge(int to, T value)
        {
            To = to;
            Value = value;
        }
        public int To { get; }
        public T Value { get; }

        [凾(256)] public static implicit operator int(WEdge<T> e) => e.To;
        public override bool Equals(object obj) => obj is WEdge<T> edge && Equals(edge);
        public bool Equals(WEdge<T> other) => To == other.To &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value);
        public override int GetHashCode() => HashCode.Combine(To, Value);
        public static bool operator ==(WEdge<T> left, WEdge<T> right) => left.Equals(right);
        public static bool operator !=(WEdge<T> left, WEdge<T> right) => !(left == right);
        public override string ToString() => (To, Value).ToString();
        [凾(256)]
        public void Deconstruct(out int to, out T value)
        {
            to = To;
            value = Value;
        }
        [凾(256)]
        public WEdge<T> Reversed(int from) => new WEdge<T>(from, Value);
    }
}
