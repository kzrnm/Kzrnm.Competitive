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
    public class Set<T> : SetBase<T, Set<T>.C<T>, Set<T>.Node, Set<T>.NodeOperator>
        where T : IComparable<T>
    {
        public Set(bool isMulti = false) : base(isMulti, new NodeOperator()) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : base(isMulti, new NodeOperator(), collection) { }

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
        public readonly struct C<Tv> : IComparable<Node> where Tv : IComparable<T>
        {
            private readonly Tv v;
            public C(Tv val) { v = val; }
            [凾(256)] public int CompareTo(Node other) => v.CompareTo(other.Value);
        }
        public readonly struct NodeOperator : ISetOperator<T, C<T>, Node>
        {
            [凾(256)] public Node Create(T item, NodeColor color) => new Node(item, color);
            [凾(256)] public T GetValue(Node node) => node.Value;
            [凾(256)] public C<T> GetCompareKey(T item) => new C<T>(item);
            [凾(256)] public int Compare(T x, T y) => x.CompareTo(y);
        }
        #endregion Operators

        #region Search
        [凾(256)] public new Node FindNode<Tv>(Tv item) where Tv : IComparable<T> => base.FindNode(new C<Tv>(item));
        [凾(256)] public bool Contains<Tv>(Tv item) where Tv : IComparable<T> => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new L()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new L()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素を返します。
        /// </summary>
        [凾(256)] public T LowerBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new L()).node.Value;
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new U()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new U()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素を返します。
        /// </summary>
        [凾(256)] public T UpperBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new U()).node.Value;

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new LR()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new LR()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素を返します。
        /// </summary>
        [凾(256)] public T ReverseLowerBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new LR()).node.Value;

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new UR()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new UR()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素を返します。
        /// </summary>
        [凾(256)] public T ReverseUpperBoundItem<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new UR()).node.Value;
        #endregion Search
    }
}
