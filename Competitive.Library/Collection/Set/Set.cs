using AtCoder.Internal;
using Kzrnm.Competitive.SetInternals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Set<T> : Set<T, DefaultComparerStruct<T>>
        where T : IComparable<T>
    {
        public Set(bool isMulti = false) : base(new DefaultComparerStruct<T>(), isMulti) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : base(collection, new DefaultComparerStruct<T>(), isMulti) { }
    }

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Set<T, TOp> : SetBase<T, T, Set<T, TOp>.Node, Set<T, TOp>.NodeOperator>
        where TOp : struct, IComparer<T>
    {
        public Set(bool isMulti = false) : this(default(TOp), isMulti) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : this(collection, default(TOp), isMulti) { }
        public Set(TOp comparer, bool isMulti = false) : base(isMulti, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public Set(IEnumerable<T> collection, TOp comparer, bool isMulti = false)
            : base(isMulti, new NodeOperator(comparer), collection) { }


        protected readonly TOp comparer;
        public class Node : SetNodeBase
        {
            public T Item;
            internal Node(T item, NodeColor color) : base(color)
            {
                Item = item;
            }
            public override string ToString() => $"Item = {Item}, Size = {Size}";
        }
        public readonly struct NodeOperator : INodeOperator<T, T, Node>
        {
            private readonly TOp comparer;
            public IComparer<T> Comparer => comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [MethodImpl(AggressiveInlining)]
            public Node Create(T item, NodeColor color) => new Node(item, color);
            [MethodImpl(AggressiveInlining)]
            public T GetValue(Node node) => node.Item;
            [MethodImpl(AggressiveInlining)]
            public void SetValue(ref Node node, T value) => node.Item = value;
            [MethodImpl(AggressiveInlining)]
            public T GetCompareKey(T item) => item;
            [MethodImpl(AggressiveInlining)]
            public int Compare(T x, T y) => comparer.Compare(x, y);
            [MethodImpl(AggressiveInlining)]
            public int Compare(Node node1, Node node2) => comparer.Compare(node1.Item, node2.Item);
            [MethodImpl(AggressiveInlining)]
            public int Compare(T value, Node node) => comparer.Compare(value, node.Item);
        }
    }
}
