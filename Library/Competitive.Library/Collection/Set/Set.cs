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
    public class Set<T> : SetBase<T, Set<T>.C<T>, Set<T>.Node, DefaultComparerStruct<T>>
        where T : IComparable<T>
    {
        public Set(bool isMulti = false) : base(isMulti, new()) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : base(isMulti, new(), collection) { }

        #region Operators
        public class Node : SetNodeBase<Node>, ISetOperator<T, C<T>, Node, DefaultComparerStruct<T>>
        {
            public T Value;
            internal Node(T item, NodeColor color) : base(color)
            {
                Value = item;
            }
            public override string ToString() => $"Value = {Value}, Size = {Size}";
            [凾(256)] public static Node Create(T item, NodeColor color) => new(item, color);
            [凾(256)] public static T GetValue(Node node) => node.Value;
            [凾(256)] public static C<T> GetCompareKey(DefaultComparerStruct<T> comparer, T item) => new(item);
        }
        public readonly struct C<Tv> : IComparable<Node> where Tv : IComparable<T>
        {
            private readonly Tv v;
            public C(Tv val) { v = val; }
            [凾(256)] public int CompareTo(Node other) => v.CompareTo(other.Value);
        }
        #endregion Operators

        #region Search
        [凾(256)] public new Node FindNode<Tv>(Tv item) where Tv : IComparable<T> => base.FindNode(new C<Tv>(item));
        [凾(256)] public bool Contains<Tv>(Tv item) where Tv : IComparable<T> => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLower()).node;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLower()).index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetLowerBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetLower()).node is { } n)
            {
                value = n.Value;
                return true;
            }
            value = default;
            return false;
        }
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpper()).node;
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpper()).index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetUpperBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpper()).node is { } n)
            {
                value = n.Value;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLowerRev()).node;
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLowerRev()).index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseLowerBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetLowerRev()).node is { } n)
            {
                value = n.Value;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public Node FindNodeReverseUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpperRev()).node;
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpperRev()).index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseUpperBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpperRev()).node is { } n)
            {
                value = n.Value;
                return true;
            }
            value = default;
            return false;
        }
        #endregion Search
    }
}
