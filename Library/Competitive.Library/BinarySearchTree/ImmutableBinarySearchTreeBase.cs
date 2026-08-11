using AtCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    namespace Internal
    {
        /// <summary>
        /// 根から平衡二分探索木を作る
        /// </summary>
        /// <typeparam name="Tr">平衡二分探索木</typeparam>
        /// <typeparam name="R">ノード参照</typeparam>
        [IsOperator]
        public interface IImmutableBbstMaker<Tr, R>
        {
            static abstract Tr Create(R node);
        }

        /// <summary>
        /// 永続化した平衡二分探索木を実装する
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="R">ノード参照</typeparam>
        /// <typeparam name="M">平衡二分探索木生成型</typeparam>
        /// <typeparam name="N">ノード操作型</typeparam>
        /// <typeparam name="TSelf">自身の型</typeparam>
        public abstract class ImmutableBinarySearchTreeBase<T, TSelf, R, M, N> : IImmutableList<T>
            where M : IImmutableBbstMaker<TSelf, R>
            where N : IBbstOp<T, R, N>
            where TSelf : ImmutableBinarySearchTreeBase<T, TSelf, R, M, N>
        {
            protected ImmutableBinarySearchTreeBase(ReadOnlySpan<T> v) : this(N.Build(v)) { }
            protected ImmutableBinarySearchTreeBase(R root)
            {
                this.root = root;
            }
            /// <summary>
            /// 二分木の根
            /// </summary>
            protected R root;
            public T this[int index]
            {
                get
                {
                    N.GetValue(root, index, out T x);
                    return x;
                }
            }

            /// <summary>
            /// <paramref name="index"/> 番目の値を <paramref name="value"/> に変更した二分探索木を返します。
            /// </summary>
            [凾(256)]
            public TSelf SetItem(int index, T value)
            {
                var t = root;
                t = N.SetValue(t, index, value);
                return M.Create(t);
            }
            IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => SetItem(index, value);

            static readonly TSelf _empty = M.Create(N.Null);
            /// <summary>
            /// 空の二分探索木を返します。
            /// </summary>
            public static TSelf Empty => _empty;

            /// <summary>
            /// 要素数を返します。
            /// </summary>
            public int Count => N.Size(root);

            /// <summary>
            /// [<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
            /// </summary>
            [凾(256)]
            public T Prod(int l, int r)
            {
                var t = root;
                return N.Prod(ref t, l, r);
            }
            [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);
            /// <summary>
            /// 総積を返します。
            /// </summary>
            public T AllProd => N.Sum(root);

            IImmutableList<T> IImmutableList<T>.Add(T value) => AddLast(value);

            /// <summary>
            /// 先頭に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf AddFirst(T item)
            {
                var t = root;
                N.AddFirst(ref t, item);
                return M.Create(t);
            }

            /// <summary>
            /// 末尾に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf AddLast(T item)
            {
                var t = root;
                N.AddLast(ref t, item);
                return M.Create(t);
            }

            /// <summary>
            /// 末尾に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf AddRange(IEnumerable<T> items)
            {
                var t = root;
                return M.Create(N.Merge(t, N.Build(items.ToArray())));
            }
            IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);


            /// <summary>
            /// <paramref name="index"/> に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf Insert(int index, T item)
            {
                var t = root;
                N.Insert(ref t, index, item);
                return M.Create(t);
            }

            /// <summary>
            /// <paramref name="index"/> に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf InsertRange(int index, IEnumerable<T> items)
            {
                var t = root;
                N.Insert(ref t, index, N.Build(items.ToArray()));
                return M.Create(t);
            }

            IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);

            IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => InsertRange(index, items);

            /// <summary>
            /// <paramref name="index"/> のノードを削除して該当のノードを返します。
            /// </summary>
            [凾(256)]
            public TSelf RemoveAt(int index)
            {
                var t = root;
                N.Erase(ref t, index);
                return M.Create(t);
            }
            IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => RemoveAt(index);

            [凾(256)]
            public TSelf RemoveRange(int index, int count)
            {
                var t = root;
                N.Erase(ref t, index, count);
                return M.Create(t);
            }
            IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);

            [凾(256)]
            public TSelf Clear() => _empty;
            IImmutableList<T> IImmutableList<T>.Clear() => Clear();

            [凾(256)]
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var v in this)
                    array[arrayIndex++] = v;
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                var t = root;
                return N.GetEnumerator(ref t);
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                var t = root;
                return N.GetEnumerator(ref t);
            }

            int IImmutableList<T>.IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            int IImmutableList<T>.LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }

            [SourceExpander.NotEmbeddingSource]
            public override string ToString() => Root?.ToString() ?? "empty";

            [SourceExpander.NotEmbeddingSource]
            object Root => N.DebugObject(root);

            /// <summary>
            /// 可能なら二分木の状態が正常か確認します
            /// </summary>
            [Conditional("DEBUG")]
            [SourceExpander.NotEmbeddingSource]
            internal void Validate() => N.Validate(root);
        }
    }
}