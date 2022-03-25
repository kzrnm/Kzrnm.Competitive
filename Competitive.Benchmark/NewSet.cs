using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive;
using Kzrnm.Competitive2.SetInternals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive2
{
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Set<T> : SetBase<T, Set<T>.C, Set<T>.Node, Set<T>.NodeOperator>
        where T : IComparable<T>
    {
        public Set(bool isMulti = false) : base(isMulti, new NodeOperator()) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : base(isMulti, new NodeOperator(), collection) { }

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
            private readonly T v;
            public C(T val) { v = val; }
            [凾(256)] public int CompareTo(Node other) => v.CompareTo(other.Value);
        }
        public readonly struct NodeOperator : ISetOperator<T, C, Node>
        {
            [凾(256)] public Node Create(T item, NodeColor color) => new Node(item, color);
            [凾(256)] public T GetValue(Node node) => node.Value;
            [凾(256)] public C GetCompareKey(T item) => new C(item);
        }
    }

    //[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    //[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    //public class Set<T, TOp> : SetBase<T, T, Set<T, TOp>.Node, Set<T, TOp>.NodeOperator>
    //    where TOp : struct, IComparer<T>
    //{
    //    public Set(bool isMulti = false) : this(default(TOp), isMulti) { }
    //    public Set(IEnumerable<T> collection, bool isMulti = false) : this(collection, default(TOp), isMulti) { }
    //    public Set(TOp comparer, bool isMulti = false) : base(isMulti, new NodeOperator(comparer))
    //    {
    //        this.comparer = comparer;
    //    }
    //    public Set(IEnumerable<T> collection, TOp comparer, bool isMulti = false)
    //        : base(isMulti, new NodeOperator(comparer), collection) { }


    //    protected readonly TOp comparer;
    //    public class Node : SetNodeBase<Node>
    //    {
    //        public T Value;
    //        internal Node(T item, NodeColor color) : base(color)
    //        {
    //            Value = item;
    //        }
    //        public override string ToString() => $"Value = {Value}, Size = {Size}";
    //    }
    //    public readonly struct NodeOperator : ISetOperator<T, T, Node>
    //    {
    //        private readonly TOp comparer;
    //        public IComparer<T> Comparer => comparer;
    //        public NodeOperator(TOp comparer)
    //        {
    //            this.comparer = comparer;
    //        }
    //        [凾(256)]
    //        public Node Create(T item, NodeColor color) => new Node(item, color);
    //        [凾(256)]
    //        public T GetValue(Node node) => node.Value;
    //        [凾(256)]
    //        public T GetCompareKey(T item) => item;
    //        [凾(256)]
    //        public int Compare(T value, Node node) => comparer.Compare(value, node.Value);
    //    }
    //}
}
