using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    namespace Internal
    {
        public interface IImmutableBbst<T, Nd, TSelf> : IImmutableList<T>
            where TSelf : IImmutableBbst<T, Nd, TSelf>
        {
            static abstract TSelf Create(Nd node);
        }

        /// <summary>
        /// 永続化した平衡二分探索木を実装する
        /// </summary>
        /// <typeparam name="T">モノイド</typeparam>
        /// <typeparam name="Nd">ノード</typeparam>
        /// <typeparam name="TSelf">自身の型</typeparam>
        public abstract class ImmutableBinarySearchTreeBase<T, Nd, TSelf> : IImmutableList<T>
            where Nd : class, IBbstNode<T, Nd>
            where TSelf : ImmutableBinarySearchTreeBase<T, Nd, TSelf>, IImmutableBbst<T, Nd, TSelf>
        {
            protected ImmutableBinarySearchTreeBase(ReadOnlySpan<T> v) : this(Nd.Build(v)) { }
            protected ImmutableBinarySearchTreeBase(Nd root)
            {
                this.root = root;
            }
            /// <summary>
            /// 二分木の根
            /// </summary>
            protected Nd root;
            public T this[int index]
            {
                get => Nd.GetValue(ref root, index);
                set => Nd.SetValue(ref root, index, value);
            }

            /// <summary>
            /// <paramref name="index"/> 番目の値を <paramref name="value"/> に変更した二分探索木を返します。
            /// </summary>
            [凾(256)]
            public TSelf SetItem(int index, T value)
            {
                var t = root;
                Nd.SetValue(ref t, index, value);
                return TSelf.Create(t);
            }
            IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => SetItem(index, value);

            static readonly TSelf _empty = TSelf.Create(null);
            /// <summary>
            /// 空の二分探索木を返します。
            /// </summary>
            public static TSelf Empty => _empty;

            /// <summary>
            /// 要素数を返します。
            /// </summary>
            public int Count => root?.Size ?? 0;

            /// <summary>
            /// [<paramref name="l"/>..<paramref name="r"/>] の総積を返します。
            /// </summary>
            [凾(256)] public T Prod(int l, int r) => Nd.Prod(ref root, l, r);
            [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);
            /// <summary>
            /// 総積を返します。
            /// </summary>
            public T AllProd => Nd.Sum(root);

            IImmutableList<T> IImmutableList<T>.Add(T value) => AddLast(value);

            /// <summary>
            /// 先頭に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf AddFirst(T item)
            {
                var t = root;
                Nd.AddFirst(ref t, item);
                return TSelf.Create(t);
            }

            /// <summary>
            /// 末尾に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf AddLast(T item)
            {
                var t = root;
                Nd.AddLast(ref t, item);
                return TSelf.Create(t);
            }

            /// <summary>
            /// 末尾に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf AddRange(IEnumerable<T> items) => TSelf.Create(Nd.Merge(root, Nd.Build(items.ToArray())));
            IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);


            /// <summary>
            /// <paramref name="index"/> に <paramref name="item"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf Insert(int index, T item)
            {
                var t = root;
                Nd.Insert(ref t, index, item);
                return TSelf.Create(t);
            }

            /// <summary>
            /// <paramref name="index"/> に <paramref name="items"/> を追加します。
            /// </summary>
            [凾(256)]
            public TSelf InsertRange(int index, IEnumerable<T> items)
            {
                var t = root;
                Nd.Insert(ref t, index, Nd.Build(items.ToArray()));
                return TSelf.Create(t);
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
                Nd.Erase(ref t, index);
                return TSelf.Create(t);
            }
            IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => RemoveAt(index);

            [凾(256)]
            public TSelf RemoveRange(int index, int count)
            {
                var t = root;
                Nd.Erase(ref t, index, count);
                return TSelf.Create(t);
            }
            IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);

            [凾(256)]
            public TSelf Clear() => TSelf.Create(null);
            IImmutableList<T> IImmutableList<T>.Clear() => Clear();

            [凾(256)]
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var v in this)
                    array[arrayIndex++] = v;
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => Nd.GetEnumerator(ref root);
            IEnumerator IEnumerable.GetEnumerator() => Nd.GetEnumerator(ref root);

            int IImmutableList<T>.IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            int IImmutableList<T>.LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
            IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
        }

    }
}