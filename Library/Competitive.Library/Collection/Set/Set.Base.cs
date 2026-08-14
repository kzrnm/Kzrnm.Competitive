using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
#if NET10_0_OR_GREATER
    public static class SetResultExt
    {
        extension<T, R, Nd>(Internal.SetResult<T, R, Nd> r) where Nd : Internal.ISetNode<T, R>
        {
            public T Value => r.Node.Value;
        }
        extension<T, R, Nd>(Internal.SetFindResult<T, R, Nd> r) where Nd : Internal.ISetNode<T, R>
        {
            public T Value => r.Node.Value;
        }
        extension<TKey, TValue, R, Nd>(Internal.SetResult<KeyValuePair<TKey, TValue>, R, Nd> r) where Nd : Internal.ISetNode<KeyValuePair<TKey, TValue>, R>
        {
            public TKey Key => r.Node.Value.Key;
            public TValue Value => r.Node.Value.Value;
        }
        extension<TKey, TValue, R, Nd>(Internal.SetFindResult<KeyValuePair<TKey, TValue>, R, Nd> r) where Nd : Internal.ISetNode<KeyValuePair<TKey, TValue>, R>
        {
            public TKey Key => r.Node.Value.Key;
            public TValue Value => r.Node.Value.Value;
        }
    }
#endif

    namespace Internal
    {
        [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
        [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
        public abstract class SetBase<T, TCmp, TOp, Nd, N> : ICollection, ICollection<T>, IReadOnlyCollection<T>
            where TCmp : IComparable<Nd>
            where TOp : struct, IComparer<T>
            where Nd : struct, ISetNode<T, int>
            where N : ISetOp<T, TCmp, TOp, Nd, int, N, PoolStructRefOp<Nd>>
        {
            /*
             * Original is SortedSet<T>
             *
             * Copyright (c) .NET Foundation and Contributors
             * Released under the MIT license
             * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
             */
            internal const string LISENCE = @"
Original is SortedSet<T>

Copyright (c) .NET Foundation and Contributors
Released under the MIT license
https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
";

            private readonly TOp op;

            public bool IsMulti { get; }
            protected int root = -1;

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => Root?.ToString() ?? "empty";

            [SourceExpander.NotEmbeddingSource]
            object Root => N.DebugObject(root);

            #region Constructor
            protected SetBase(bool isMulti, TOp op)
            {
                IsMulti = isMulti;
                this.op = op;
            }
            protected SetBase(bool isMulti, TOp op, IEnumerable<T> collection) : this(isMulti, op)
            {
                root = N.ConstructRootFromSortedArray(N.InitArray(collection, op, isMulti), -1);
            }
            #endregion Constructor
            public T Min => root < 0 ? default : N.GetValue(N.MinNode(root));
            public T Max => root < 0 ? default : N.GetValue(N.MaxNode(root));

            #region Search
            /// <summary>
            /// <paramref name="item"/> が含まれていれば返します。
            /// </summary>
            [凾(256)]
            public SetFindResult<T, int, Nd>? FindNode(T item)
                => FindNode(N.GetCompareKey(op, item));
            /// <summary>
            /// <paramref name="key"/> が含まれていれば返します。
            /// </summary>
            [凾(256)]
            public SetFindResult<T, int, Nd>? FindNode<TKey>(TKey key) where TKey : IComparable<Nd>
                => (N.Find(root, key) is { NodeRef: >= 0 } n) ? n : null;

            [凾(256)]
            public SetFindResult<T, int, Nd> FindByIndex(int index)
                => N.GetByIndex(root, index);

            /// <summary>
            /// <paramref name="item"/> 以上/超えるの要素のノードとインデックスを返します。
            /// </summary>
            /// <param name="item">検索する要素</param>
            /// <param name="bop">二分探索の判定オペレーター</param>
            [凾(256)]
            public SetFindResult<T, int, Nd> BinarySearch<TBOp>(T item, TBOp bop = default)
                   where TBOp : struct, ISetBinarySearchOperator
                => N.BinarySearch(root, N.GetCompareKey(op, item), bop);

            [凾(256)]
            protected SetFindResult<T, int, Nd> BinarySearch<TKey, TBOp>(TKey item, TBOp bop = default)
                where TKey : IComparable<Nd>
                where TBOp : struct, ISetBinarySearchOperator
                => N.BinarySearch(root, item, bop);


            /// <summary>
            /// <paramref name="item"/> 以上の最初のノードを返します。
            /// </summary>
            [凾(256)]
            public SetFindResult<T, int, Nd> FindNodeLowerBound(T item) => BinarySearch<SetLower>(item);
            /// <summary>
            /// <paramref name="item"/> 以上の最初のインデックスを返します。なければ Count を返します。
            /// </summary>
            [凾(256)]
            public int LowerBoundIndex(T item) => BinarySearch<SetLower>(item).Index;
            /// <summary>
            /// <paramref name="item"/> 以上の最初の要素があれば <paramref name="value"/> で返します。
            /// </summary>
            /// <returns>要素を取得できたかどうか</returns>
            [凾(256)]
            public bool TryGetLowerBound(T item, out T value)
            {
                if (BinarySearch<SetLower>(item) is { NodeRef: >= 0, Node.Value: var v })
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
            [凾(256)]
            public SetFindResult<T, int, Nd> FindNodeUpperBound(T item) => BinarySearch<SetUpper>(item);
            /// <summary>
            /// <paramref name="item"/> を超える最初のインデックスを返します。なければ Count を返します。
            /// </summary>
            [凾(256)]
            public int UpperBoundIndex(T item) => BinarySearch<SetUpper>(item).Index;
            /// <summary>
            /// <paramref name="item"/> を超える最初の要素があれば <paramref name="value"/> で返します。
            /// </summary>
            /// <returns>要素を取得できたかどうか</returns>
            [凾(256)]
            public bool TryGetUpperBound(T item, out T value)
            {
                if (BinarySearch<SetUpper>(item) is { NodeRef: >= 0, Node.Value: var v })
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
            [凾(256)]
            public SetFindResult<T, int, Nd> FindNodeReverseLowerBound(T item) => BinarySearch<SetLowerRev>(item);
            /// <summary>
            /// <paramref name="item"/> 以下の最後のインデックスを返します。なければ -1 を返します。
            /// </summary>
            [凾(256)]
            public int ReverseLowerBoundIndex(T item) => BinarySearch<SetLowerRev>(item).Index;
            /// <summary>
            /// <paramref name="item"/> 以下の最後の要素があれば <paramref name="value"/> で返します。
            /// </summary>
            /// <returns>要素を取得できたかどうか</returns>
            [凾(256)]
            public bool TryGetReverseLowerBound(T item, out T value)
            {
                if (BinarySearch<SetLowerRev>(item) is { NodeRef: >= 0, Node.Value: var v })
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
            [凾(256)]
            public SetFindResult<T, int, Nd> FindNodeReverseUpperBound(T item) => BinarySearch<SetUpperRev>(item);
            /// <summary>
            /// <paramref name="item"/> 未満の最後のインデックスを返します。なければ -1 を返します。
            /// </summary>
            [凾(256)]
            public int ReverseUpperBoundIndex(T item) => BinarySearch<SetUpperRev>(item).Index;
            /// <summary>
            /// <paramref name="item"/> 未満の最後の要素があれば <paramref name="value"/> で返します。
            /// </summary>
            /// <returns>要素を取得できたかどうか</returns>
            [凾(256)]
            public bool TryGetReverseUpperBound(T item, out T value)
            {
                if (BinarySearch<SetUpperRev>(item) is { NodeRef: >= 0, Node.Value: var v })
                {
                    value = v;
                    return true;
                }
                value = default;
                return false;
            }
            #endregion Search

            #region Enumerate
            [凾(256)]
            public IEnumerable<T> Reversed()
            {
                ValueEnumerator e = new(root, true, -1);
                while (e.MoveNext()) yield return e.Current;
            }

            /// <summary>
            /// ノードを列挙する。
            /// </summary>
            /// <param name="reverse">逆順で列挙する</param>
            /// <returns></returns>
            [凾(256)]
            public IEnumerable<SetResult<T, int, Nd>> EnumerateNode(bool reverse = false)
                => EnumerateNode(-1, reverse);

            /// <summary>
            /// <paramref name="item"/> 以上のノードを列挙する。
            /// </summary>
            /// <returns></returns>
            [凾(256)]
            public IEnumerable<SetResult<T, int, Nd>> EnumerateNodeUpper(T item)
            {
                var n = FindNodeLowerBound(item);
                return n.NodeRef < 0 ? [] : EnumerateNode(n.NodeRef, false);
            }

            /// <summary>
            /// <paramref name="item"/> 以下のノードを逆順で列挙する。
            /// </summary>
            /// <returns></returns>
            [凾(256)]
            public IEnumerable<SetResult<T, int, Nd>> EnumerateNodeLower(T item)
            {
                var n = FindNodeReverseLowerBound(item);
                return n.NodeRef < 0 ? [] : EnumerateNode(n.NodeRef, true);
            }

            /// <summary>
            /// <paramref name="i"/> 以降のノードを列挙する。
            /// </summary>
            /// <returns></returns>
            [凾(256)]
            public IEnumerable<SetResult<T, int, Nd>> EnumerateNodeSkip(int i)
            {
                var n = FindByIndex(i);
                return n.NodeRef < 0 ? [] : EnumerateNode(n.NodeRef, false);
            }

            /// <summary>
            /// <paramref name="i"/> までのノードを逆順で列挙する。
            /// </summary>
            /// <returns></returns>
            [凾(256)]
            public IEnumerable<SetResult<T, int, Nd>> EnumerateNodeRev(int i)
            {
                var n = FindByIndex(i);
                return n.NodeRef < 0 ? [] : EnumerateNode(n.NodeRef, true);
            }

            /// <summary>
            /// <paramref name="from"/> 以上/以下のノードを列挙する。<paramref name="from"/> がnullならばすべて列挙する。
            /// </summary>
            /// <param name="from">列挙開始するノードの値</param>
            /// <param name="reverse">以上ではなく以下を列挙する</param>
            /// <returns></returns>
            [凾(256)]
            protected IEnumerable<SetResult<T, int, Nd>> EnumerateNode(int from = -1, bool reverse = false)
            {
                var e = N.GetEnumerator(root, reverse, from);
                while (e.MoveNext())
                {
                    var (r, v) = e.Current;
                    yield return new(r, v);
                }
            }
            #endregion Enumerate

            #region ICollection<T> members
            void ICollection<T>.Add(T item) => N.Add(ref root, item, op, IsMulti);

            /// <summary>
            /// <paramref name="item"/> を追加します。変更されない(Set が Multi でなく追加済)場合は <see langword="false" /> を返す。
            /// </summary>
            [凾(256)]
            public bool Add(T item) => N.Add(ref root, item, op, IsMulti) >= 0;

            [凾(256)] public SetResult<T, int, Nd> RemoveAt(int i) => N.RemoveAt(ref root, i);
            [凾(256)] public SetResult<T, int, Nd>? GetAndRemove(T item) => GetAndRemove(N.GetCompareKey(op, item));
            [凾(256)] public SetResult<T, int, Nd>? GetAndRemove(TCmp item) => N.Remove(ref root, item);
            [凾(256)] public bool Remove(T item) => N.Remove(ref root, item, op).NodeRef >= 0;
            [凾(256)] public bool Remove(TCmp item) => N.Remove(ref root, item).NodeRef >= 0;
            [凾(256)] public bool Remove(SetFindResult<T, int, Nd> r) => RemoveAt(r.Index).NodeRef >= 0;

            public void Clear()
            {
                root = -1;
            }
            public bool Contains(T item) => FindNode(item) != null;
            void ICollection.CopyTo(Array array, int index) => CopyTo((T[])array, index);
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var item in this) array[arrayIndex++] = item;
            }

            #endregion ICollection<T> members

            bool ICollection<T>.IsReadOnly => false;
            bool ICollection.IsSynchronized => false;
            object ICollection.SyncRoot => this;
            public int Count => N.Size(root);


            [凾(256)] public ISetOp<T, Nd, int, N, PoolStructRefOp<Nd>>.Enumerator GetNodeEnumerator(bool reverse = false, int start = -1) => N.GetEnumerator(root, reverse, start);
            [凾(256)] public ValueEnumerator GetEnumerator() => new(root);
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => new ValueEnumerator(root);
            IEnumerator IEnumerable.GetEnumerator() => new ValueEnumerator(root);

            public struct ValueEnumerator : IEnumerator<T>
            {
                private ISetOp<T, Nd, int, N, PoolStructRefOp<Nd>>.Enumerator inner;
                internal ValueEnumerator(int root, bool reverse = false, int startNode = -1)
                {
                    inner = new(root, reverse, startNode);
                }

                public T Current => inner.Current.Node.Value;
                object IEnumerator.Current => Current;

                [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
                public void Dispose() { }
                [凾(256)]
                public bool MoveNext() => inner.MoveNext();
                public void Reset() => throw new NotSupportedException();
            }
        }

        public readonly record struct SetResult<T, R, Nd>(R NodeRef, [property: DebuggerBrowsable(DebuggerBrowsableState.RootHidden)] Nd Node) where Nd : ISetNode<T, R>;
        public readonly record struct SetFindResult<T, R, Nd>(int Index, R NodeRef, [property: DebuggerBrowsable(DebuggerBrowsableState.RootHidden)] Nd Node) where Nd : ISetNode<T, R>;
        public interface ISetNode<R>
        {
            R Parent { get; set; }
            R Left { get; set; }
            R Right { get; set; }
            bool IsBlack { get; set; }
            int Size { get; set; }

            [SourceExpander.NotEmbeddingSource]
            string ToStringImpl() => $"Size = {Size}";
        }
        public interface ISetNode<T, R> : ISetNode<R>
        {
            T Value { get; }
        }


        public interface ISetPOp<T, TCmp, TOp, Nd, N> : ISetOp<T, TCmp, TOp, Nd, int, N, PoolStructRefOp<Nd>>
            where TCmp : IComparable<Nd>
            where TOp : struct, IComparer<T>
            where Nd : struct, ISetNode<T, int>
            where N : ISetPOp<T, TCmp, TOp, Nd, N>
        {
            /// <summary>
            /// 単一の値を持つノードを作成します。
            /// </summary>
            static abstract Nd CreateNode(T v, bool isBlack);

            [凾(256)]
            static int ISetOp<T, Nd, int, N, PoolStructRefOp<Nd>>.Create(T v, bool isBlack)
            {
                StructPool<Nd>.Default.Rent(out var i) = N.CreateNode(v, isBlack);
                return i;
            }
        }


        [IsOperator]
        public interface ISetOp<T, TCmp, TOp, Nd, R, N, C> : ISetOp<T, Nd, R, N, C>
            where TCmp : IComparable<Nd>
            where TOp : struct, IComparer<T>
            where Nd : ISetNode<T, R>
            where N : ISetOp<T, TCmp, TOp, Nd, R, N, C>
            where C : IPoolRefOp<Nd, R>
        {
            static abstract TCmp GetCompareKey(TOp comparer, T item);

            /// <summary>
            /// <paramref name="col"/> をソートします。
            /// </summary>
            /// <param name="col"></param>
            /// <param name="op"></param>
            /// <param name="multi"></param>
            /// <returns></returns>
            static virtual ReadOnlySpan<T> InitArray(IEnumerable<T> col, TOp op, bool multi)
            {
                T[] arr;
                int count;
                arr = col.ToArray();
                if (arr.Length == 0)
                    return arr;

                Array.Sort(arr, op);
                if (multi)
                {
                    count = arr.Length;
                }
                else
                {
                    count = 1;
                    for (int i = 1; i < arr.Length; i++)
                    {
                        if (op.Compare(arr[i], arr[i - 1]) != 0)
                        {
                            arr[count++] = arr[i];
                        }
                    }
                }
                return arr.AsSpan(0, count);
            }

            /// <summary>
            /// <paramref name="item"/> にマッチするノードを削除してそのノードの値を返します。
            /// </summary>
            [凾(256)]
            static virtual SetResult<T, R, Nd> Remove(ref R root, T item, TOp comparer)
                => N.Remove(ref root, N.GetCompareKey(comparer, item));

            /// <summary>
            /// <paramref name="item"/> を可能なら追加します。追加できたらノード参照を返します。
            /// 追加できなければ(Set が <paramref name="isMulti"/> でなく追加済ならば) null 参照を返します。
            /// </summary>
            [凾(256)]
            static virtual R Add(ref R root, T item, TOp comparer, bool isMulti)
                => N.Add(ref root, N.GetCompareKey(comparer, item), item, comparer, isMulti);

            /// <summary>
            /// <paramref name="item"/> を可能なら追加します。追加できたらノード参照を返します。
            /// 追加できなければ(Set が <paramref name="isMulti"/> でなく追加済ならば) null 参照を返します。
            /// </summary>
            [凾(256)]
            static virtual R Add(ref R root, TCmp key, T item, TOp comparer, bool isMulti)
            {
                if (C.IsNull(root))
                {
                    root = N.Create(item, true);
                    return root;
                }
                R current = root;
                R parent = C.Null;
                R grandParent = C.Null;
                R greatGrandParent = C.Null;
                int order = 0;
                while (!C.IsNull(current))
                {
                    order = key.CompareTo(C.Load(current));
                    if (order == 0 && !isMulti)
                    {
                        C.Load(root).IsBlack = true;
                        return C.Null;
                    }
                    if (Is4Node(current))
                    {
                        Split4Node(current);
                        if (!C.IsNull(parent) && !C.Load(parent).IsBlack)
                        {
                            InsertionBalance(ref root, current, ref parent, grandParent, greatGrandParent);
                        }
                    }
                    greatGrandParent = grandParent;
                    grandParent = parent;
                    parent = current;
                    current = order < 0 ? C.Load(current).Left : C.Load(current).Right;
                }
                R node = N.Create(item, false);

                ref Nd p = ref C.Load(parent);
                if (order >= 0)
                    SetRight(parent, node);
                else
                    SetLeft(parent, node);

                if (!p.IsBlack) InsertionBalance(ref root, node, ref parent, grandParent, greatGrandParent);
                C.Load(root).IsBlack = true;
                return node;
            }
        }
        [IsOperator]
        public interface ISetOp<T, Nd, R, N, C>
            where Nd : ISetNode<T, R>
            where N : ISetOp<T, Nd, R, N, C>
            where C : IPoolRefOp<Nd, R>
        {

            static abstract R Create(T v, bool isBlack);
            #region Set Operations
            readonly struct IdxCmp(int i) : IComparableS
            {
                public int CompareTo(Nd d, int ix) => i.CompareTo(ix);

                [SourceExpander.NotEmbeddingSource]
                public readonly override string ToString() => $"Index:{i}";
            }
            readonly struct CS<S>(S s) : IComparableS where S : IComparable<Nd>
            {
                [凾(256)]
                public int CompareTo(Nd d, int ix) => s.CompareTo(d);
                [SourceExpander.NotEmbeddingSource]
                public readonly override string ToString() => $"{s}";
            }

            interface IComparableS
            {
                int CompareTo(Nd d, int ix);
            }

            [凾(256)]
            static virtual SetFindResult<T, R, Nd> GetByIndex(R root, int i)
                => FindS(root, new IdxCmp(i));

            /// <summary>
            /// <paramref name="key"/> にマッチするノードとインデックスを返します。
            /// </summary>
            /// <param name="root">赤黒木の根</param>
            /// <param name="key">検索する要素</param>
            [凾(256)]
            static virtual SetFindResult<T, R, Nd> Find<TKey>(R root, TKey key)
                where TKey : IComparable<Nd>
                => FindS(root, new CS<TKey>(key));

            /// <summary>
            /// <paramref name="key"/> にマッチするノードとインデックスを返します。
            /// </summary>
            /// <param name="root">赤黒木の根</param>
            /// <param name="key">検索する要素</param>
            [凾(256)]
            static SetFindResult<T, R, Nd> FindS<TKey>(R root, TKey key)
                where TKey : IComparableS
            {
                R left = C.Null, right = C.Null;
                int size = 0;
                if (C.IsNull(root))
                    goto NULL;
                R current = root;
                ref Nd d = ref C.Load(current);
                size = d.Size;
                int li = -1;
                int ri = size;
                int ci = N.Size(d.Left);
                while (true)
                {
                    int cp = key.CompareTo(d, ci);
                    if (cp < 0)
                    {
                        right = current;
                        ri = ci;
                        current = d.Left;
                        if (C.IsNull(current))
                            break;
                        d = ref C.Load(current);
                        ci -= N.Size(d.Right) + 1;
                    }
                    else if (cp == 0)
                    {
                        return new(ci, current, d);
                    }
                    else
                    {
                        left = current;
                        li = ci;
                        current = d.Right;
                        if (C.IsNull(current))
                            break;
                        d = ref C.Load(current);
                        ci += N.Size(d.Left) + 1;
                    }
                }
            NULL:
                return new(-1, C.Null, default);
            }

            /// <summary>
            /// <paramref name="key"/> 以上/超えるの要素のノードとインデックスを返します。
            /// </summary>
            /// <param name="root">赤黒木の根</param>
            /// <param name="key">検索する要素</param>
            /// <param name="bop">二分探索の判定オペレーター</param>
            [凾(256)]
            static virtual SetFindResult<T, R, Nd> BinarySearch<TKey, TBOp>(R root, TKey key, TBOp bop)
                where TKey : IComparable<Nd>
                where TBOp : struct, ISetBinarySearchOperator
            {
                R left = C.Null, right = C.Null;
                int size = 0;
                if (C.IsNull(root))
                    goto NULL;
                R current = root;
                ref Nd d = ref C.Load(current);
                size = d.Size;
                int li = -1;
                int ri = size;
                int ci = N.Size(d.Left);
                while (true)
                {
                    int cp = key.CompareTo(d);
                    if (bop.IntoLeft(cp))
                    {
                        right = current;
                        ri = ci;
                        current = d.Left;
                        if (C.IsNull(current))
                            break;
                        d = ref C.Load(current);
                        ci -= N.Size(d.Right) + 1;
                    }
                    else
                    {
                        left = current;
                        li = ci;
                        current = d.Right;
                        if (C.IsNull(current))
                            break;
                        d = ref C.Load(current);
                        ci += N.Size(d.Left) + 1;
                    }
                }
                if (bop.ReturnLeft && li >= 0)
                    return new(li, left, C.Load(left));
                else if (!bop.ReturnLeft && ri < size)
                    return new(ri, right, C.Load(right));
            NULL:
                return new(bop.ReturnLeft ? -1 : size, C.Null, default);
            }

            [凾(256)]
            static virtual SetResult<T, R, Nd> RemoveAt(ref R root, int i)
                => RemoveS(ref root, new IdxCmp(i));


            /// <summary>
            /// <paramref name="key"/> にマッチするノードを削除してそのノードの値を返します。
            /// </summary>
            [凾(256)]
            static virtual SetResult<T, R, Nd> Remove<TKey>(ref R root, TKey key)
                where TKey : IComparable<Nd>
                => RemoveS(ref root, new CS<TKey>(key));

            /// <summary>
            /// <paramref name="key"/> にマッチするノードを削除してそのノードの値を返します。
            /// </summary>
            [凾(256)]
            static SetResult<T, R, Nd> RemoveS<TCmp>(ref R root, TCmp key) where TCmp : IComparableS
            {
                if (C.IsNull(root))
                    goto NO;
                R current = root;
                R parent = C.Null;
                R grandParent = C.Null;
                R match = C.Null;
                R parentOfMatch = C.Null;
                bool foundMatch = false;
                int ci = N.Size(C.Load(current).Left);
                while (true)
                {
                    if (Is2Node(current))
                        Fix2Node(ref root, match, ref parentOfMatch, current, parent, grandParent);
                    int order = foundMatch ? -1 : key.CompareTo(C.Load(current), ci);
                    if (order == 0)
                    {
                        foundMatch = true;
                        match = current;
                        parentOfMatch = parent;
                    }
                    grandParent = parent;
                    parent = current;
                    if (order < 0)
                    {
                        current = C.Load(current).Left;
                        if (C.IsNull(current)) break;
                        ci -= N.Size(C.Load(current).Right) + 1;
                    }
                    else
                    {
                        current = C.Load(current).Right;
                        if (C.IsNull(current)) break;
                        ci += N.Size(C.Load(current).Left) + 1;
                    }
                }
                if (!C.IsNull(match))
                    ReplaceNode(ref root, match, parentOfMatch, parent, grandParent);
                if (!C.IsNull(root))
                    C.Load(root).IsBlack = true;

                if (!C.IsNull(match))
                {
                    var matchNode = C.Load(match);
                    C.Free(match);
                    return new(match, matchNode);
                }
            NO:
                return new(C.Null, default);
            }

            [凾(256)]
            static void Fix2Node(ref R root, R match, ref R parentOfMatch, R current, R parent, R grandParent)
            {
                Debug.Assert(Is2Node(current));
                if (C.IsNull(parent))
                {
                    C.Load(current).IsBlack = false;
                }
                else
                {
                    var sibling = GetSibling(parent, current);
                    if (!C.Load(sibling).IsBlack)
                    {
                        Debug.Assert(C.Load(parent).IsBlack);
                        if (EqualityComparer<R>.Default.Equals(C.Load(parent).Right, sibling)) RotateLeft(parent);
                        else RotateRight(parent);

                        C.Load(parent).IsBlack = false;
                        C.Load(sibling).IsBlack = true;
                        ReplaceChildOrRoot(ref root, grandParent, parent, sibling);
                        grandParent = sibling;
                        if (EqualityComparer<R>.Default.Equals(parent, match)) parentOfMatch = sibling;
                        sibling = GetSibling(parent, current);
                    }
                    Debug.Assert(!C.IsNull(sibling) && C.Load(sibling).IsBlack);
                    if (Is2Node(sibling))
                    {
                        Merge2Nodes(parent);
                    }
                    else
                    {
                        R newGrandParent = Rotate(parent, GetRotation(parent, current, sibling));
                        C.Load(newGrandParent).IsBlack = C.Load(parent).IsBlack;
                        C.Load(parent).IsBlack = true;
                        C.Load(current).IsBlack = false;
                        ReplaceChildOrRoot(ref root, grandParent, parent, newGrandParent);
                        if (EqualityComparer<R>.Default.Equals(parent, match))
                        {
                            parentOfMatch = newGrandParent;
                        }
                    }
                }
            }

            [凾(256)]
            static void InsertionBalance(ref R root, R current, ref R parent, R grandParent, R greatGrandParent)
            {
                Debug.Assert(!C.IsNull(parent));
                Debug.Assert(!C.IsNull(grandParent));
                bool parentIsOnRight = EqualityComparer<R>.Default.Equals(C.Load(grandParent).Right, parent);
                bool currentIsOnRight = EqualityComparer<R>.Default.Equals(C.Load(parent).Right, current);
                R newChildOfGreatGrandParent;
                if (parentIsOnRight == currentIsOnRight)
                {
                    newChildOfGreatGrandParent = currentIsOnRight ? RotateLeft(grandParent) : RotateRight(grandParent);
                }
                else
                {
                    newChildOfGreatGrandParent = currentIsOnRight ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent);
                    parent = greatGrandParent;
                }
                C.Load(grandParent).IsBlack = false;
                C.Load(newChildOfGreatGrandParent).IsBlack = true;
                ReplaceChildOrRoot(ref root, greatGrandParent, grandParent, newChildOfGreatGrandParent);

            }

            [凾(256)]
            static void ReplaceChildOrRoot(ref R root, R parent, R child, R newChild)
            {
                if (!C.IsNull(parent))
                    ReplaceChild(parent, child, newChild);
                else
                {
                    root = newChild;
                    if (!C.IsNull(root))
                        C.Load(root).Parent = C.Null;
                }
            }

            [凾(256)]
            static void ReplaceNode(ref R root, R match, R parentOfMatch, R successor, R parentOfSuccessor)
            {
                Debug.Assert(!C.IsNull(match));
                ref Nd m = ref C.Load(match);
                if (EqualityComparer<R>.Default.Equals(successor, match))
                {
                    Debug.Assert(C.IsNull(m.Right));
                    successor = m.Left;
                }
                else
                {
                    ref Nd s = ref C.Load(successor);
                    Debug.Assert(!C.IsNull(parentOfSuccessor));
                    Debug.Assert(C.IsNull(s.Left));
                    Debug.Assert((C.IsNull(s.Right) && !s.IsBlack) || (s.IsBlack && !C.Load(s.Right).IsBlack));

                    if (!C.IsNull(s.Right))
                        C.Load(s.Right).IsBlack = true;

                    if (!EqualityComparer<R>.Default.Equals(parentOfSuccessor, match))
                    {
                        SetLeft(parentOfSuccessor, s.Right);
                        SetRight(successor, m.Right);
                    }
                    SetLeft(successor, m.Left);
                }
                if (!C.IsNull(successor))
                {
                    C.Load(successor).IsBlack = m.IsBlack;
                }
                ReplaceChildOrRoot(ref root, parentOfMatch, match, successor);
            }
            static virtual R ConstructRootFromSortedArray(ReadOnlySpan<T> arr, R redNode)
            {
                R root;
                switch (arr.Length)
                {
                    case 0:
                        return C.Null;
                    case 1:
                        root = N.Create(arr[0], true);
                        if (!C.IsNull(redNode))
                        {
                            SetLeft(root, redNode);
                        }
                        break;
                    case 2:
                        root = N.Create(arr[0], true);
                        var c2R = N.Create(arr[^1], false);

                        SetRight(root, c2R);
                        if (!C.IsNull(redNode))
                        {
                            SetLeft(root, redNode);
                        }
                        break;
                    case 3:
                        root = N.Create(arr[1], true);
                        var c3L = N.Create(arr[0], true);
                        var c3R = N.Create(arr[^1], true);

                        SetLeft(root, c3L);
                        SetRight(root, c3R);
                        if (!C.IsNull(redNode))
                        {
                            SetLeft(c3L, redNode);
                        }
                        break;
                    default:
                        int midpt = (arr.Length - 1) / 2;
                        root = N.Create(arr[midpt], true);
                        var cL = N.ConstructRootFromSortedArray(arr[..midpt], redNode);
                        var cR = arr.Length % 2 == 0 ?
                            N.ConstructRootFromSortedArray(arr[(midpt + 2)..], N.Create(arr[midpt + 1], false)) :
                            N.ConstructRootFromSortedArray(arr[(midpt + 1)..], C.Null);

                        SetLeft(root, cL);
                        SetRight(root, cR);
                        break;
                }
                return root;
            }
            #endregion Set Operations

            #region Node Operations
            [凾(256)]
            static virtual int Size(R t) => C.IsNull(t) ? 0 : C.Load(t).Size;
            [凾(256)]
            static virtual T GetValue(R t) => C.IsNull(t) ? default : C.Load(t).Value;

            [凾(256)]
            static virtual R MinNode(R cur)
            {
                if (C.IsNull(cur))
                    return cur;
                R ch;
                while (!C.IsNull(ch = C.Load(cur).Left))
                    cur = ch;
                return cur;
            }
            [凾(256)]
            static virtual R MaxNode(R cur)
            {
                if (C.IsNull(cur))
                    return cur;
                R ch;
                while (!C.IsNull(ch = C.Load(cur).Right))
                    cur = ch;
                return cur;
            }

            [凾(256)]
            static bool Is2Node(R t)
            {
                ref Nd d = ref C.Load(t);
                return d.IsBlack && (C.IsNull(d.Left) || C.Load(d.Left).IsBlack) && (C.IsNull(d.Right) || C.Load(d.Right).IsBlack);
            }
            [凾(256)]
            static bool Is4Node(R t)
            {
                ref Nd d = ref C.Load(t);
                return !C.IsNull(d.Left) && !C.Load(d.Left).IsBlack && !C.IsNull(d.Right) && !C.Load(d.Right).IsBlack;
            }

            [凾(256)]
            static void SetLeft(R t, R c)
            {
                C.Load(t).Left = c;
                if (!C.IsNull(c))
                    C.Load(c).Parent = t;

                UpdateRoots(t);
            }

            [凾(256)]
            static void SetRight(R t, R c)
            {
                C.Load(t).Right = c;
                if (!C.IsNull(c))
                    C.Load(c).Parent = t;

                UpdateRoots(t);
            }

            [凾(256)]
            static void UpdateRoots(R t)
            {
                for (R cur = t; !C.IsNull(cur) && UpdateSize(cur); cur = C.Load(cur).Parent)
                {
                    ref Nd d = ref C.Load(cur);
                    if (!C.IsNull(d.Parent))
                    {
                        ref Nd p = ref C.Load(d.Parent);
                        if (!EqualityComparer<R>.Default.Equals(p.Left, cur) && !EqualityComparer<R>.Default.Equals(p.Right, cur))
                        {
                            d.Parent = C.Null;
                            break;
                        }
                    }
                }
            }

            [凾(256)]
            static bool UpdateSize(R t)
            {
                ref Nd d = ref C.Load(t);
                var oldsize = d.Size;
                var size = 1 + N.Size(d.Left) + N.Size(d.Right);
                d.Size = size;
                return oldsize != size;
            }

            [凾(256)]
            static TreeRotation GetRotation(R t, R current, R sibling)
            {
                ref Nd d = ref C.Load(t);
                ref Nd s = ref C.Load(sibling);
                Debug.Assert((!C.IsNull(s.Left) && !C.Load(s.Left).IsBlack) || (!C.IsNull(s.Right) && !C.Load(s.Right).IsBlack));
                bool currentIsLeftChild = EqualityComparer<R>.Default.Equals(d.Left, current);
                return !C.IsNull(s.Left) && !C.Load(s.Left).IsBlack ?
                    (currentIsLeftChild ? TreeRotation.RightLeft : TreeRotation.Right) :
                    (currentIsLeftChild ? TreeRotation.Left : TreeRotation.LeftRight);
            }

            [凾(256)]
            static R GetSibling(R t, R node)
            {
                ref Nd d = ref C.Load(t);
                Debug.Assert(!C.IsNull(node));
                Debug.Assert(EqualityComparer<R>.Default.Equals(node, d.Left) ^ EqualityComparer<R>.Default.Equals(node, d.Right));

                return EqualityComparer<R>.Default.Equals(node, d.Left) ? d.Right : d.Left;
            }
            [凾(256)]
            static void Split4Node(R t)
            {
                ref Nd d = ref C.Load(t);
                Debug.Assert(!C.IsNull(d.Left));
                Debug.Assert(!C.IsNull(d.Right));

                d.IsBlack = false;
                C.Load(d.Left).IsBlack = true;
                C.Load(d.Right).IsBlack = true;
            }

            [凾(256)]
            static R Rotate(R t, TreeRotation rotation)
            {
                ref Nd d = ref C.Load(t);
                switch (rotation)
                {
                    case TreeRotation.Right:
                        ref Nd removeRed1 = ref C.Load(C.Load(d.Left).Left);
                        Debug.Assert(!removeRed1.IsBlack);
                        removeRed1.IsBlack = true;
                        return RotateRight(t);
                    case TreeRotation.Left:
                        ref Nd removeRed2 = ref C.Load(C.Load(d.Right).Right);
                        Debug.Assert(!removeRed2.IsBlack);
                        removeRed2.IsBlack = true;
                        return RotateLeft(t);
                    case TreeRotation.RightLeft:
                        Debug.Assert(!C.Load(C.Load(d.Right).Left).IsBlack);
                        return RotateRightLeft(t);
                    case TreeRotation.LeftRight:
                        Debug.Assert(!C.Load(C.Load(d.Left).Right).IsBlack);
                        return RotateLeftRight(t);
                    default:
                        Debug.Fail("ここには来ないはず");
                        return t;
                }
            }

            [凾(256)]
            static R RotateLeft(R t)
            {
                ref Nd d = ref C.Load(t);
                var c = d.Right;
                ref Nd child = ref C.Load(c);
                SetRight(t, child.Left);
                SetLeft(c, t);
                return c;
            }
            [凾(256)]
            static R RotateLeftRight(R t)
            {
                ref Nd d = ref C.Load(t);
                var c = d.Left;
                ref Nd child = ref C.Load(c);
                var g = child.Right;
                ref Nd grandChild = ref C.Load(g);

                SetLeft(t, grandChild.Right);
                SetRight(g, t);
                SetRight(c, grandChild.Left);
                SetLeft(g, c);
                return g;
            }
            [凾(256)]
            static R RotateRight(R t)
            {
                ref Nd d = ref C.Load(t);
                var c = d.Left;
                ref Nd child = ref C.Load(c);
                SetLeft(t, child.Right);
                SetRight(c, t);
                return c;
            }
            [凾(256)]
            static R RotateRightLeft(R t)
            {
                ref Nd d = ref C.Load(t);
                var c = d.Right;
                ref Nd child = ref C.Load(c);
                var g = child.Left;
                ref Nd grandChild = ref C.Load(g);

                SetRight(t, grandChild.Left);
                SetLeft(g, t);
                SetLeft(c, grandChild.Right);
                SetRight(g, c);
                return g;
            }

            [凾(256)]
            static void Merge2Nodes(R t)
            {
                ref Nd d = ref C.Load(t);
                Debug.Assert(!d.IsBlack);
                Debug.Assert(Is2Node(d.Left));
                Debug.Assert(Is2Node(d.Right));

                // Combine two 2-nodes into a 4-node.
                d.IsBlack = true;
                C.Load(d.Left).IsBlack = false;
                C.Load(d.Right).IsBlack = false;
            }

            [凾(256)]
            static void ReplaceChild(R t, R child, R newChild)
            {
                ref Nd d = ref C.Load(t);
                if (EqualityComparer<R>.Default.Equals(d.Left, child))
                    SetLeft(t, newChild);
                else
                    SetRight(t, newChild);
            }
            enum TreeRotation : byte
            {
                Left = 1,
                Right = 2,
                RightLeft = 3,
                LeftRight = 4,
            }
            #endregion Node Operations

            static virtual Enumerator GetEnumerator(R root, bool reverse, R startNode)
                => new(root, reverse, startNode);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
            public struct Enumerator : IEnumerator<(R NodeRef, Nd Node)>
            {
                internal readonly R root;
                [SourceExpander.NotEmbeddingSource]
                SetNodeConv RootDebug => new(root, C.Load(root));
                readonly Deque<R> stack;
                R current;

                readonly bool reverse;
                internal Enumerator(R root, bool reverse, R startNode)
                {
                    this.root = root;
                    stack = new(2 * Log2(N.Size(root) + 1));
                    current = C.Null;
                    this.reverse = reverse;
                    if (C.IsNull(startNode)) IntializeAll();
                    else Intialize(startNode);
                }
                [凾(256)]
                void IntializeAll()
                {
                    var node = root;
                    while (!C.IsNull(node))
                    {
                        ref Nd d = ref C.Load(node);
                        var next = reverse ? d.Right : d.Left;
                        stack.AddLast(node);
                        node = next;
                    }
                }
                [凾(256)]
                void Intialize(R startNode)
                {
                    if (C.IsNull(startNode))
                        throw new InvalidOperationException(nameof(startNode) + "is null");
                    current = C.Null;
                    if (reverse)
                        InitializeReverse(startNode);
                    else
                        InitializeNormal(startNode);
                }
                [凾(256)]
                void InitializeNormal(R node)
                {
                    while (!C.IsNull(node))
                    {
                        while (!C.IsNull(node))
                        {
                            stack.AddFirst(node);
                            var parent = C.Load(node).Parent;
                            if (C.IsNull(parent) || EqualityComparer<R>.Default.Equals(C.Load(parent).Right, node)) { node = parent; break; }
                            node = parent;
                        }
                        while (!C.IsNull(node))
                        {
                            var parent = C.Load(node).Parent;
                            if (C.IsNull(parent) || EqualityComparer<R>.Default.Equals(C.Load(parent).Left, node)) { node = parent; break; }
                            node = parent;
                        }
                    }
                }
                [凾(256)]
                void InitializeReverse(R node)
                {
                    while (!C.IsNull(node))
                    {
                        while (!C.IsNull(node))
                        {
                            stack.AddFirst(node);
                            var parent = C.Load(node).Parent;
                            if (C.IsNull(parent) || EqualityComparer<R>.Default.Equals(C.Load(parent).Left, node)) { node = parent; break; }
                            node = parent;
                        }
                        while (!C.IsNull(node))
                        {
                            var parent = C.Load(node).Parent;
                            if (C.IsNull(parent) || EqualityComparer<R>.Default.Equals(C.Load(parent).Right, node)) { node = parent; break; }
                            node = parent;
                        }
                    }
                }

                [凾(256)]
                static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
                public (R NodeRef, Nd Node) Current
                {
                    [凾(256)]
                    get
                    {
                        Debug.Assert(!C.IsNull(current));
                        return (current, C.Load(current));
                    }
                }

                [凾(256)]
                public bool MoveNext()
                {
                    if (stack.Count == 0)
                    {
                        current = C.Null;
                        return false;
                    }
                    current = stack.PopLast();
                    R node;
                    {
                        ref Nd d = ref C.Load(current);
                        node = reverse ? d.Left : d.Right;
                    }
                    while (!C.IsNull(node))
                    {
                        ref Nd d = ref C.Load(node);
                        var next = reverse ? d.Right : d.Left;
                        stack.AddLast(node);
                        node = next;
                    }
                    return true;
                }

                object IEnumerator.Current => Current;
                public void Dispose() { }
                public void Reset() => throw new NotSupportedException();
            }

            [SourceExpander.NotEmbeddingSource]
            static virtual object DebugObject(R t) => C.IsNull(t) ? null : C.Load(t);
        }

        [SourceExpander.NotEmbeddingSource]
        internal class SetNodeConv(object r, object n)
        {
            public object NodeReference => r;
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object Node => n;
            public override string ToString() => $"{Node} @{NodeReference}";
            public static SetNodeConv Load<Nd>(int t)
                where Nd : struct => new(t, t < 0 ? null : PoolStructRefOp<Nd>.Load(t));
        }

        [IsOperator]
        public interface ISetBinarySearchOperator
        {
            /// <summary>
            /// 左側を返す
            /// </summary>
            bool ReturnLeft { get; }

            /// <summary>
            /// 左側に潜る
            /// </summary>
            bool IntoLeft(int order);
        }
        public readonly struct SetLower : ISetBinarySearchOperator
        {
            public bool ReturnLeft => false;
            [凾(256)]
            public bool IntoLeft(int order) => order <= 0;
        }
        public readonly struct SetUpper : ISetBinarySearchOperator
        {
            public bool ReturnLeft => false;
            [凾(256)]
            public bool IntoLeft(int order) => order < 0;
        }
        public readonly struct SetLowerRev : ISetBinarySearchOperator
        {
            public bool ReturnLeft => true;
            [凾(256)]
            public bool IntoLeft(int order) => order < 0;
        }
        public readonly struct SetUpperRev : ISetBinarySearchOperator
        {
            public bool ReturnLeft => true;
            [凾(256)]
            public bool IntoLeft(int order) => order <= 0;
        }
    }
}