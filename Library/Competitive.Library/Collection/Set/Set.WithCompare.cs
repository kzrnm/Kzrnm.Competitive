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
    public class Set<T, TOp> : SetBase<T, Set<T, TOp>.C, Set<T, TOp>.Node, Set<T, TOp>.NodeOperator>
        where TOp : struct, IComparer<T>
    {
        public Set(bool isMulti = false) : this(default(TOp), isMulti) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : this(collection, default(TOp), isMulti) { }
        public Set(TOp comparer, bool isMulti = false) : base(isMulti, new NodeOperator(comparer))
        {
            this.comparer = comparer;
        }
        public Set(IEnumerable<T> collection, TOp comparer, bool isMulti = false)
            : base(isMulti, new NodeOperator(comparer), collection)
        {
            this.comparer = comparer;
        }

        protected readonly TOp comparer;

        #region Operators
        public class Node : SetNodeBase<Node>
        {
            public T Value;
            internal Node(T item, NodeColor color) : base(color)
            {
                Value = item;
            }
            public override string ToString() => $"Value = {Value}, Size = {Size}";
        }
        public readonly struct C : IComparable<Node>
        {
            private readonly TOp op;
            private readonly T v;
            public C(TOp op, T val) { this.op = op; v = val; }
            [凾(256)] public int CompareTo(Node other) => op.Compare(v, other.Value);
        }
        public readonly struct NodeOperator : ISetOperator<T, C, Node>
        {
            private readonly TOp op;
            public NodeOperator(TOp op) { this.op = op; }
            [凾(256)] public Node Create(T item, NodeColor color) => new Node(item, color);
            [凾(256)] public T GetValue(Node node) => node.Value;
            [凾(256)] public C GetCompareKey(T item) => new C(op, item);
            public int Compare(T x, T y) => op.Compare(x, y);
        }
        #endregion Operators
    }
}
