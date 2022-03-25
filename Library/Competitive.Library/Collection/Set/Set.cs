using AtCoder.Internal;
using Kzrnm.Competitive.SetInternals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
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
        public class Node : SetNodeBase<Node>
        {
            public T Value;
            internal Node(T item, NodeColor color) : base(color)
            {
                Value = item;
            }
            public override string ToString() => $"Value = {Value}, Size = {Size}";
        }
        public readonly struct NodeOperator : ISetOperator<T, T, Node>
        {
            private readonly TOp comparer;
            public IComparer<T> Comparer => comparer;
            public NodeOperator(TOp comparer)
            {
                this.comparer = comparer;
            }
            [凾(256)]
            public Node Create(T item, NodeColor color) => new Node(item, color);
            [凾(256)]
            public T GetValue(Node node) => node.Value;
            [凾(256)]
            public T GetCompareKey(T item) => item;
            [凾(256)]
            public int Compare(T value, Node node) => comparer.Compare(value, node.Value);
            [凾(256)]
            public int Compare(T x, T y) => comparer.Compare(x, y);
        }
    }
}
