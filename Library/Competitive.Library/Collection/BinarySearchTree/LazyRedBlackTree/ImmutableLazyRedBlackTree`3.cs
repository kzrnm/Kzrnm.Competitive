using AtCoder.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 永続遅延伝搬赤黒木
    // https://ei1333.github.io/library/structure/bbst/lazy-red-black-tree.hpp

    /// <summary>
    /// 永続遅延伝搬反転可能赤黒木
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ImmutableLazyRedBlackTree<T, F, TOp> : IImmutableList<T>
        where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
    {
        private static LazyReversibleBinarySearchTreeNodeOperator<T, F, TOp, LazyRedBlackTreeNode<T, F>, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> rb => default;

        private LazyRedBlackTreeNode<T, F> root;

        public static ImmutableLazyRedBlackTree<T, F, TOp> Empty { get; } = new ImmutableLazyRedBlackTree<T, F, TOp>();
        protected ImmutableLazyRedBlackTree() { }

        private ImmutableLazyRedBlackTree(LazyRedBlackTreeNode<T, F> root) { this.root = root; }
        public ImmutableLazyRedBlackTree(IEnumerable<T> v) : this(v.ToArray()) { }
        public ImmutableLazyRedBlackTree(T[] v) : this(v.AsSpan()) { }
        public ImmutableLazyRedBlackTree(ReadOnlySpan<T> v) : this(rb.im.Build(v)) { }

        [凾(256)] public T Prod(int l, int r) => rb.Prod(ref root, l, r);
        [凾(256)] public T Slice(int l, int length) => Prod(l, l + length);
        public T AllProd => rb.Sum(root);
        public int Count => rb.Size(root);
        public T this[int index] => rb.im.GetValue(ref root, index);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Add(T item)
        {
            var t = root;
            rb.AddLast(ref t, item);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> AddRange(IEnumerable<T> items)
        {
            var t = rb.im.Merge(root, rb.im.Build(items.ToArray()));
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items)
            => AddRange(items);


        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Clear() => Empty;
        IImmutableList<T> IImmutableList<T>.Clear() => Empty;

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Insert(int index, T item)
        {
            var t = root;
            rb.Insert(ref t, index, item);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
            => Insert(index, element);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> InsertRange(int index, IEnumerable<T> items)
        {
            var t = root;
            var (t1, t2) = rb.im.Split(t, index);
            rb.Merge3(t1, rb.im.Build(items.ToArray()), t2);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
            => InsertRange(index, items);


        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> RemoveAt(int index)
        {
            var t = root;
            rb.Erase(ref t, index);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
            => RemoveAt(index);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> RemoveRange(int index, int count)
        {
            var t = root;
            var (t1, _, t3) = rb.Split3(t, index, index + count);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(rb.im.Merge(t1, t3));
        }
        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count)
            => RemoveRange(index, count);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> SetItem(int index, T value)
        {
            var t = root;
            rb.im.SetValue(ref t, index, value);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value)
            => SetItem(index, value);

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Apply(int l, int r, F f)
        {
            var t = root;
            rb.Apply(ref t, l, r, f);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Reverse()
        {
            var t = root;
            rb.Reverse(ref t, 0, Count);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }

        [凾(256)]
        public ImmutableLazyRedBlackTree<T, F, TOp> Reverse(int l, int r)
        {
            var t = root;
            rb.Reverse(ref t, l, r);
            return new ImmutableLazyRedBlackTree<T, F, TOp>(t);
        }
        [凾(256)]
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var v in this)
                array[arrayIndex++] = v;
        }


        public bool Contains(T _) { throw new NotSupportedException(); }
        public int IndexOf(T _) { throw new NotSupportedException(); }

        public bool Remove(T _) { throw new NotSupportedException(); }
        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            throw new NotSupportedException();
        }
        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            throw new NotSupportedException();
        }

        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer)
        {
            throw new NotSupportedException();
        }
        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match)
        {
            throw new NotSupportedException();
        }
        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }
        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) { throw new NotSupportedException(); }


        [凾(256)]
        public LazyRedBlackTreeEnumerator<T, F, TOp, LazyRedBlackTreeNodeOperator<T, F, TOp, TCp>> GetEnumerator()
            => rb.im.GetEnumerator(root);
        [凾(256)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        [凾(256)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct TCp : ICopyOperator<LazyRedBlackTreeNode<T, F>>
        {
            [凾(256)]
            public LazyRedBlackTreeNode<T, F> Copy(LazyRedBlackTreeNode<T, F> t) => new LazyRedBlackTreeNode<T, F>(t.Key, t.Lazy)
            {
                Level = t.Level,
                Color = t.Color,
                Left = t.Left,
                Right = t.Right,
                Sum = t.Sum,
                Size = t.Size,
                IsReverse = t.IsReverse,
            };
        }
    }
}
