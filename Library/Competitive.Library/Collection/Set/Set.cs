using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Set<T> : Set<T, DefaultComparerStruct<T>>
        where T : IComparable<T>
    {
        public Set(bool isMulti = false) : base(isMulti) { }
        public Set(IEnumerable<T> collection, bool isMulti = false) : base(collection, isMulti) { }


        #region Search
        [凾(256)] public new SetFindResult<T, int, SetNode<T>>? FindNode<Tv>(Tv item) where Tv : IComparable<T> => base.FindNode(new C<Tv>(item));
        [凾(256)] public bool Contains<Tv>(Tv item) where Tv : IComparable<T> => FindNode(item) != null;
        /// <summary>
        /// <paramref name="item"/> 以上の最初のノードを返します。
        /// </summary>
        [凾(256)] public SetFindResult<T, int, SetNode<T>> FindNodeLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLower());
        /// <summary>
        /// <paramref name="item"/> 以上の最初のインデックスを返します。なければ Count を返します。
        /// </summary>
        [凾(256)] public int LowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLower()).Index;
        /// <summary>
        /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetLowerBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetLower()) is { NodeRef: >= 0, Node.Value: var v })
            {
                value = v;
                return true;
            }
            value = default;
            return false;
        }
        /// <summary>
        /// <paramref name="item"/> を超える最初のノードを返します。
        /// </summary>
        [凾(256)] public SetFindResult<T, int, SetNode<T>> FindNodeUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpper());
        /// <summary>
        /// <paramref name="item"/> を超える最初のインデックスを返します。なければ Count を返します。
        /// </summary>
        [凾(256)] public int UpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpper()).Index;
        /// <summary>
        /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetUpperBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpper()) is { NodeRef: >= 0, Node.Value: var v })
            {
                value = v;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// <paramref name="item"/> 以下の最後のノードを返します。
        /// </summary>
        [凾(256)] public SetFindResult<T, int, SetNode<T>> FindNodeReverseLowerBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLowerRev());
        /// <summary>
        /// <paramref name="item"/> 以下の最後のインデックスを返します。なければ -1 を返します。
        /// </summary>
        [凾(256)] public int ReverseLowerBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetLowerRev()).Index;
        /// <summary>
        /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseLowerBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetLowerRev()) is { NodeRef: >= 0, Node.Value: var v })
            {
                value = v;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// <paramref name="item"/> 未満の最後のノードを返します。
        /// </summary>
        [凾(256)] public SetFindResult<T, int, SetNode<T>> FindNodeReverseUpperBound<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpperRev());
        /// <summary>
        /// <paramref name="item"/> 未満の最後のインデックスを返します。なければ -1 を返します。
        /// </summary>
        [凾(256)] public int ReverseUpperBoundIndex<Tv>(Tv item) where Tv : IComparable<T> => BinarySearch(new C<Tv>(item), new SetUpperRev()).Index;
        /// <summary>
        /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
        /// </summary>
        /// <returns>要素を取得できたかどうか</returns>
        [凾(256)]
        public bool TryGetReverseUpperBound<Tv>(Tv item, out T value) where Tv : IComparable<T>
        {
            if (BinarySearch(new C<Tv>(item), new SetUpperRev()) is { NodeRef: >= 0, Node.Value: var v })
            {
                value = v;
                return true;
            }
            value = default;
            return false;
        }
        #endregion Search

        /// <summary>
        /// <paramref name="item"/> 以上のノードを列挙する。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public IEnumerable<SetResult<T, int, SetNode<T>>> EnumerateNodeUpper<Tv>(Tv item)
            where Tv : IComparable<T>
        {
            var n = FindNodeLowerBound(item);
            return n.NodeRef < 0 ? [] : EnumerateNode(n.NodeRef, false);
        }

        /// <summary>
        /// <paramref name="item"/> 以下のノードを逆順で列挙する。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public IEnumerable<SetResult<T, int, SetNode<T>>> EnumerateNodeLower<Tv>(Tv item)
            where Tv : IComparable<T>
        {
            var n = FindNodeReverseLowerBound(item);
            return n.NodeRef < 0 ? [] : EnumerateNode(n.NodeRef, true);
        }
    }

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Set<T, TOp> : SetBase<T, Set<T, TOp>.C, TOp, SetNode<T>, Set<T, TOp>.Op>
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
        public struct Op : ISetPOp<T, C, TOp, SetNode<T>, Op>
        {
            [凾(256)]
            public static SetNode<T> CreateNode(T v, bool isBlack)
                => new(v, isBlack);

            [凾(256)]
            public static C GetCompareKey(TOp comparer, T item)
                => new(comparer, item);
        }

        public readonly record struct C(TOp op, T v) : IComparable<SetNode<T>>
        {
            [凾(256)] public int CompareTo(SetNode<T> other) => op.Compare(v, other.Value);

            [SourceExpander.NotEmbeddingSource]
            public readonly override string ToString() => $"{v}";
        }
        public readonly record struct C<Tv>(Tv v) : IComparable<SetNode<T>> where Tv : IComparable<T>
        {
            [凾(256)] public int CompareTo(SetNode<T> other) => v.CompareTo(other.Value);

            [SourceExpander.NotEmbeddingSource]
            public readonly override string ToString() => $"{v}";
        }
        #endregion Operators
    }

    namespace Internal
    {
#pragma warning disable IDE0251 // メンバーを 'readonly' にする
        [StructLayout(LayoutKind.Auto)]
        public struct SetNode<T> : ISetNode<T, int>
        {
            public T Value { get; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Parent { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Left { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Right { get; set; }
            public bool IsBlack { get; set; }
            public int Size { get; set; }

            [SourceExpander.NotEmbeddingSource]
            readonly object DebugParent => SetNodeConv.Load<SetNode<T>>(Parent);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugLeft => SetNodeConv.Load<SetNode<T>>(Left);
            [SourceExpander.NotEmbeddingSource]
            readonly object DebugRight => SetNodeConv.Load<SetNode<T>>(Right);

            internal SetNode(T item, bool isBlack)
            {
                Parent = Left = Right = -1;
                Size = 1;
                IsBlack = isBlack;
                Value = item;
            }

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => $"Value = {Value} Size = {Size}";
        }
#pragma warning restore IDE0251 // メンバーを 'readonly' にする
    }
}
