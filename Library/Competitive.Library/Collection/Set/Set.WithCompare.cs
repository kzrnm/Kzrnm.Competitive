using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Set<T, TOp> : SetBase<T, Set<T, TOp>.C, Set<T, TOp>.Node, TOp>
        where TOp : struct, IComparer<T>
    {
        public Set(bool isMulti = false) : this(new TOp(), isMulti) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : this(collection, new TOp(), isMulti) { }
        public Set(TOp comparer, bool isMulti = false) : base(isMulti, comparer)
        {
            this.comparer = comparer;
        }
        public Set(IEnumerable<T> collection, TOp comparer, bool isMulti = false)
            : base(isMulti, comparer, collection)
        {
            this.comparer = comparer;
        }

        protected readonly TOp comparer;

        #region Operators
        public class Node : SetNodeBase<Node>, ISetOperator<T, C, Node, TOp>
        {
            public T Value;
            internal Node(T item, NodeColor color) : base(color)
            {
                Value = item;
            }
#if !LIBRARY
            [SourceExpander.NotEmbeddingSource]
#endif
            public override string ToString() => $"Value = {Value}, Size = {Size}";
            [凾(256)] public static Node Create(T item, NodeColor color) => new(item, color);
            [凾(256)] public static T GetValue(Node node) => node.Value;
            [凾(256)] public static C GetCompareKey(TOp op, T item) => new(op, item);
        }
        public readonly struct C : IComparable<Node>
        {
            private readonly TOp op;
            private readonly T v;
            public C(TOp op, T val) { this.op = op; v = val; }
            [凾(256)] public int CompareTo(Node other) => op.Compare(v, other.Value);
        }
        #endregion Operators
    }
}
